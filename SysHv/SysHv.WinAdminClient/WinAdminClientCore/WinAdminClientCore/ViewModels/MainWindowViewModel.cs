using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Microsoft.AspNetCore.SignalR.Client;
using SysHv.Client.Common.DTOs.SensorOutput;
using WinAdminClientCore.Collections;
using WinAdminClientCore.Models;
using WinAdminClientCore.UIHelpers;

namespace WinAdminClientCore.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private DispatcherizedObservableCollection<ProcessorLoadDTO> _cpuLoad;

        public DispatcherizedObservableCollection<ProcessorLoadDTO> CpuLoad
        {
            get => _cpuLoad;
            set
            {
                _cpuLoad = value;
                OnPropertyChanged(nameof(CpuLoad));
            }
        }


        private double _lastLecture;
        private double _trend;

        public MainWindowViewModel()
        {
            /*CpuLoad = new DispatcherizedObservableCollection<DefaultComputerInfo>()
            {
                new DefaultComputerInfo() { DisplayName = "asd"},
                new DefaultComputerInfo() { DisplayName = "qwe"},
                new DefaultComputerInfo() { DisplayName = "zxc"}
            };*/

            CpuLoad = new DispatcherizedObservableCollection<ProcessorLoadDTO>();

            var connection = new HubConnectionBuilder()
                .WithUrl(PropertiesManager.SignalRServer)
                .Build();

            connection.On<RuntimeInfoDTO>("UpdateReceived", ondata);
            connection.StartAsync();




            LastHourSeries = new SeriesCollection
            {
                new LineSeries
                {
                    AreaLimit = -10,
                    Values = new ChartValues<ObservableValue>
                    {
                        new ObservableValue(3),
                        new ObservableValue(5),
                        new ObservableValue(6),
                        new ObservableValue(7),
                        new ObservableValue(3),
                        new ObservableValue(4),
                        new ObservableValue(2),
                        new ObservableValue(5),
                        new ObservableValue(8),
                        new ObservableValue(3),
                        new ObservableValue(5),
                        new ObservableValue(6),
                        new ObservableValue(7),
                        new ObservableValue(3),
                        new ObservableValue(4),
                        new ObservableValue(2),
                        new ObservableValue(5),
                        new ObservableValue(8)
                    }
                }
            };
            _trend = 8;






            /*Task.Run(() =>
            {
                var r = new Random();
                while (true)
                {
                    Thread.Sleep(500);
                    _trend += (r.NextDouble() > 0.3 ? 1 : -1)*r.Next(0, 5);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        LastHourSeries[0].Values.Add(new ObservableValue(_trend));
                        LastHourSeries[0].Values.RemoveAt(0);
                        SetLecture();
                    });
                }
            });*/
        }

        public SeriesCollection LastHourSeries { get; set; }

        public double LastLecture
        {
            get { return _lastLecture; }
            set
            {
                _lastLecture = value;
                OnPropertyChanged("LastLecture");
            }
        }

        private void SetLecture()
        {
            var target = ((ChartValues<ObservableValue>)LastHourSeries[0].Values).Last().Value;
            var step = (target - _lastLecture) / 4;


            Task.Run(() =>
            {
                for (var i = 0; i < 4; i++)
                {
                    Thread.Sleep(100);
                    LastLecture += step;
                }
                LastLecture = target;
            });

        }

        void ondata(RuntimeInfoDTO o)
        {
            //MessageBox.Show(o.ToString());
            Console.WriteLine(o.GetType());
            foreach (var dto in o.CouLoad)
            {
                LastHourSeries[0].Values.Add(new ObservableValue(dto.Value ?? 0));
            }
            for(int i = 0, len = LastHourSeries[0].Values.Count / 3; i < len; i++)
            {
                LastHourSeries[0].Values.RemoveAt(0);
            }
            //MessageBox.Show(o.CouLoad.Count.ToString());

        }
    }
}