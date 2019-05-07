using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json;
using WinAdminClientCore.DataHelpers;
using WinAdminClientCore.Dtos;
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
            // nehooya sebe resharper umeet
            get => _logInCommand ??= new RelayCommand.RelayCommand(
                p => CanLogin(),
                p => RunMainWindow());
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

        private void RunMainWindow()
        {
            if (!LogIn())
                return;

            var mainWindow = new MainWindow(new MainWindowViewModel());
            //mainWindow.DataContext = new MainWindowViewModel();
            mainWindow.Show();

            _window.Close();
        }

        private bool LogIn()
        {
            PropertiesManager.RememberMe = RememberMe;
            if (RememberMe)
            {
                PropertiesManager.UserName = UserName;
                PropertiesManager.Password = Password;
            }
            var server = PropertiesManager.SignalRServer;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(server);

                RequestBuilder.SetJsonAsAcceptable(client);

                var content = RequestBuilder.GenerateLoginBody(UserName, Password);

                HttpResponseMessage result;
                try
                {
                    result = client.PostAsync("/api/account/login", content).Result;

                    if (result.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        MessageBox.Show("wrong user credentials");
                        return false;
                    }

                    var token = JsonConvert.DeserializeObject<TokenDTO>(result.Content.ReadAsStringAsync().Result);
                    if (!string.IsNullOrEmpty(token.Token))
                        PropertiesManager.Token = token.Token;
                    else
                    {
                        MessageBox.Show("failed to aquire session token");
                        return false;
                    }

                    return true;
                }
                catch (Exception e)
                {
                    MessageBox.Show("a connection problem encountered");
                    return false;
                }
            }
        }

        #endregion
    }
}