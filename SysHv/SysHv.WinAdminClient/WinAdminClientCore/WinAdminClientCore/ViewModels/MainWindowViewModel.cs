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
            //var obj = JsonConvert.DeserializeObject<SensorResponse>(o.ToString());

            var settings = new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Error};

            var obj = TryConvert(o.ToString());// = JsonConvert.DeserializeObject<SensorResponse>(o.ToString(), settings);
            ProcessTypes(obj);

            //var obj1 = Convert.ChangeType(obj.Item2, obj.Item1);

            //MessageBox.Show(obj1.GetType().ToString());


            //Console.WriteLine(o.GetType());
            /*foreach (var dto in o.CouLoad)
            {
                Computers[0].AddTemperatureDot(new ObservableValue(dto.Value ?? 0));
            }*/
            /*for(int i = 0, len = LastHourSeries[0].Values.Count / 3; i < len; i++)
            {
                LastHourSeries[0].Values.RemoveAt(0);
            }*/
            //MessageBox.Show(o.CouLoad.Count.ToString());

        }

        void ProcessTypes(Tuple<Type, object> incoming)
        {
            var obj = Convert.ChangeType(incoming.Item2, incoming.Item1);

            switch (obj)
            {
                case SensorResponse val:
                    Computers[0].AddTemperatureDot(new ObservableValue((double)val.Value));
                    return;
            }
        }

        public Tuple<Type, object> TryConvert(string json)
        {
            var settings = new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Error };
            var sensorTypes = new[] { typeof(DefaultComputerInfo), typeof(SensorResponse)};

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