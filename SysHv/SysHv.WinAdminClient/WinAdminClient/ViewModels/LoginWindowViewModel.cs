using System.Windows;
using System.Windows.Input;

namespace WinAdminClient.ViewModels
{
    public class LoginWindowViewModel : ViewModelBase
    {
        #region Fields

        private string _userName;

        private string _password;

        private ICommand _logInCommand;

        private Window _window;

        #endregion

        #region Properties

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand LogInCommand
        {
            get => _logInCommand ?? (_logInCommand = new RelayCommand.RelayCommand(
                       p => CanLogin(),
                       p => LogIn())
                   );
        }

        #endregion

        #region Constructors

        public LoginWindowViewModel(Window window)
        {
            _window = window;
            UserName = Properties.Credentials.Default.UserName;
            Password = Properties.Credentials.Default.Password;
        }

        #endregion

        #region Private Methods

        private bool CanLogin()
        {
            return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);
        }

        private void LogIn()
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();

            _window.Close();
        }

        #endregion
    }
}