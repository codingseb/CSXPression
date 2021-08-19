using CSXPression;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace WindowsTests
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ExpressionEvaluator evaluator = new();

        public string Expression { get; set; }

        public string Result { get; set; }

        public ICommand ExecuteCommand => new RelayCommand(_ =>
        {
            try
            {
                Result = evaluator.Evaluate(Expression)?.ToString() ?? "null";
            }
            catch(Exception exception)
            {
                Result = exception.ToString();
            }
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
