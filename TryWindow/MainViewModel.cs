using CSXPression;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TryWindow
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ExpressionEvaluator evaluator = new();
        private CancellationTokenSource cancellationTokenSource;

        public string Expression { get; set; }

        public string Result { get; set; }

        public string ExecutionTime { get; set; }

        public int Iterations { get; set; } = 1;

        public bool InExecution { get; set; }

        public ICommand ExecuteCommand => new RelayCommand(async _ =>
        {
            InExecution = true;
            Stopwatch stopWatch = new Stopwatch();
            cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Token.ThrowIfCancellationRequested();

            Result = await Task.Run(() =>
            {
                string innerResult = "null or Empty";
                stopWatch.Start();

                try
                {
                    Func<object> compiledFunc = evaluator.Compile<object>(Expression);

                    for (int i = 0; i < Iterations && !cancellationTokenSource.Token.IsCancellationRequested; i++)
                    {
                        innerResult = compiledFunc()?.ToString() ?? "null or Empty";
                    }

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

            InExecution = false;
        });

        private RelayCommand cancelCommand;
        public ICommand CancelCommand => cancelCommand ??= new RelayCommand(_ => cancellationTokenSource.Cancel());

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

        private void Cancel(object commandParameter)
        {
        }
        #endregion
    }
}
