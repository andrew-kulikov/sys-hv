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
using Newtonsoft.Json;
using SysHv.Client.Common.DTOs.SensorOutput;
using WinAdminClientCore.Collections;
using WinAdminClientCore.Enums;
using WinAdminClientCore.Models;
using WinAdminClientCore.UIHelpers;

namespace WinAdminClientCore.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private DispatcherizedObservableCollection<ComputerInfoViewModel> _computers;

        public DispatcherizedObservableCollection<ComputerInfoViewModel> Computers
        {
            get => _computers;
            set
            {
                _computers = value;
                OnPropertyChanged(nameof(Computers));
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

            Computers = new DispatcherizedObservableCollection<ComputerInfoViewModel>()
            {
                new ComputerInfoViewModel()
            };

            var connection = new HubConnectionBuilder()
                .WithUrl($"{PropertiesManager.SignalRServer}{PropertiesManager.Hub}")
                .Build();

            connection.On<object>("UpdateReceived", ondata);
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




        }

        public SeriesCollection LastHourSeries { get; set; }


        void ondata(object o)
        {
            var obj = TryConvert(o.ToString());
            ProcessTypes(obj);
        }

        void ProcessTypes(Tuple<Type, object> incoming)
        {
            var obj = Convert.ChangeType(incoming.Item2, incoming.Item1);

            if (obj is SensorResponse)
                switch ((SensorContract)(obj as SensorResponse).SensorId)
                {
                    case SensorContract.SingleDoubleValue:

                        break;
                }

        }

        public Tuple<Type, object> TryConvert(string json)
        {
            var settings = new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Error };
            var sensorTypes = new[] {typeof(SensorResponse), typeof(CPULoadSensorDto) };

            foreach (var type in sensorTypes)
            {
                try
                {
                    return new Tuple<Type, object>(type, JsonConvert.DeserializeObject(json, type, settings));
                }
                catch (Exception e)
                {
                    continue;
                }
            }
            return new Tuple<Type, object>(typeof(string), "");
        }
    }
}