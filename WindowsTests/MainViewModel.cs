using CSXPression;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WindowsTests
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ExpressionEvaluator evaluator = new();
        private CancellationTokenSource cancellationTokenSource;

        public string Expression { get; set; }

        public string Result { get; set; }

        public string ExecutionTime { get; set; }

        public int Iterations { get; set; } = 1;

        public ICommand ExecuteCommand => new RelayCommand(async _ =>
        {
            Stopwatch stopWatch = new Stopwatch();
            cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Token.ThrowIfCancellationRequested();
            
            Result = await Task.Run(() =>
            {
                string innerResult = "null or Empty";
                stopWatch.Start();

                try
                {
                    for (int i = 0; i < Iterations && !cancellationTokenSource.Token.IsCancellationRequested; i++)
                        innerResult = evaluator.Evaluate(Expression)?.ToString() ?? "null or Empty";

                    return innerResult;
                }
                catch (Exception exception)
                {
                    return exception.ToString();
                }
                finally
                {
                    stopWatch.Stop();
                }
            }, cancellationTokenSource.Token).ConfigureAwait(true);

            ExecutionTime = $"Execution time : {stopWatch.Elapsed}";
        });

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Singleton          

        private static MainViewModel instance;

        public static MainViewModel Instance => instance ??= new MainViewModel();

        private MainViewModel()
        { }

        #endregion
    }
}
