using System;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;
using SysHv.Client.Common.DTOs.SensorOutput;
using WinAdminClientCore.Collections;
using WinAdminClientCore.Models;

namespace WinAdminClientCore.ViewModels
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

            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:51610/monitoringHub")
                .Build();

            connection.On<RuntimeInfoDTO>("UpdateReceived", ondata);
            connection.StartAsync();
        }

        void ondata(RuntimeInfoDTO o)
        {
            MessageBox.Show(o.ToString());
            Console.WriteLine(o.GetType());
        }
    }
}