using System;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json;
using WinAdminClientCore.DataHelpers;
using WinAdminClientCore.Dtos;
using WinAdminClientCore.UIHelpers;

namespace WinAdminClientCore.ViewModels
{
    public class RegisterClientViewModel : ViewModelBase
    {
        #region Fields

        private string _ip;

        private string _name;

        private string _description;

        private ICommand _registerCommand;


        #endregion

        #region Properties

        public string Ip
        {
            get => _ip;
            set
            {
                _ip = value;
                OnPropertyChanged(nameof(Ip));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public ICommand Register
        {
            get
            {
                return _registerCommand ??= new RelayCommand.RelayCommand(p => CanRegister(), p => RegisterClient());
            }
        }

        private bool CanRegister()
        {
            var can = !string.IsNullOrEmpty(Ip) && !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Description);
            return can;
        }

        private void RegisterClient()
        {
            using (var client = new HttpClient())
            {
                var serverAddress = PropertiesManager.SignalRServer;
                client.BaseAddress = new Uri(serverAddress);

                RequestBuilder.SetAuthToken(client);

                var content = new StringContent(
                    JsonConvert.SerializeObject(new RegisterClientDto() { Name = Name, Description = Description, Ip = Ip }),
                    Encoding.UTF8,
                    "application/json");

                var result = client.PostAsync($"/api/client/register", content).Result;


                if (!result.IsSuccessStatusCode)
                    MessageBox.Show("error creating a new client");
            }
        }

        #endregion
    }
}