using System.Windows;
using System.Windows.Input;
using WinAdminClientCore.UIHelpers;

namespace WinAdminClientCore.ViewModels
{
    public class LoginWindowViewModel : ViewModelBase
    {
        #region Fields

        private string _userName;

        private string _password;

        private bool _rememberMe;

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

        public bool RememberMe
        {
            get => _rememberMe;
            set
            {
                _rememberMe = value;
                OnPropertyChanged(nameof(RememberMe));
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
            RememberMe = PropertiesManager.RememberMe;
            if (RememberMe)
            {
                UserName = PropertiesManager.UserName;
                Password = PropertiesManager.Password;
            }

        }

        #endregion

        #region Private Methods

        private bool CanLogin()
        {
            return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password);
        }

        private void LogIn()
        {
            PropertiesManager.RememberMe = RememberMe;
            if (RememberMe)
            {
                PropertiesManager.UserName = UserName;
                PropertiesManager.Password = Password;
            }

            var mainWindow = new MainWindow(new MainWindowViewModel());
            //mainWindow.DataContext = new MainWindowViewModel();
            mainWindow.Show();

            _window.Close();
        }

        #endregion
    }
}