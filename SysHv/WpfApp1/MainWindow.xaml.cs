using System;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;
using SysHv.Client.Common.DTOs.SensorOutput;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

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
