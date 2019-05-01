using System;
using WinAdminClient.Collections;
using WinAdminClient.Models;
using WinAdminClient.Views;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;


namespace WinAdminClient.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private DispatcherizedObservableCollection<DefaultComputerInfo> _computers;

        public DispatcherizedObservableCollection<DefaultComputerInfo> Computers
        {
            get => _computers;
            set
            {
                _computers = value;
                OnPropertyChanged(nameof(Computers));
            }
        }

        public MainWindowViewModel()
        {
            Computers = new DispatcherizedObservableCollection<DefaultComputerInfo>()
            {
                new DefaultComputerInfo() { DisplayName = "asd"},
                new DefaultComputerInfo() { DisplayName = "qwe"},
                new DefaultComputerInfo() { DisplayName = "zxc"}
            };

            var connection = new HubConnection("https://localhost:8000/monitoringHub");
            var hubProxy = connection.CreateHubProxy("hub");
            hubProxy.On<string>("UpdateReceived", (str) => Console.WriteLine("asd" + str));
        }
    }
}