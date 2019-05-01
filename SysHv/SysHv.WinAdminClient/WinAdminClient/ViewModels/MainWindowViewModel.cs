using System;
using System.Windows;
using WinAdminClient.Collections;
using WinAdminClient.Models;
using Microsoft.AspNetCore.SignalR.Client;


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

            //var connection = new HubConnection("http://localhost:51610/monitoringHub");
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:53353/ChatHub%22")
                    .Build();
            var hubProxy = connection.CreateHubProxy("MonitoringHub");
            hubProxy.On<string>("UpdateReceived", ondata);
        }

        void ondata(string o)
        {
            MessageBox.Show(o.ToString());
            Console.WriteLine(o.GetType());
        }
    }
}