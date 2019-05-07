using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using SysHv.Server.DAL.Models;
using WinAdminClientCore.Collections;
using WinAdminClientCore.DataHelpers;
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
                .WithUrl($"{PropertiesManager.SignalRServer}{PropertiesManager.Hub}", options =>
                    {
                        options.AccessTokenProvider = () => Task.FromResult(PropertiesManager.Token);
                    })
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
            var obj = JsonConvert.DeserializeObject<SensorResponse>(o.ToString());
            ProcessResponse(obj);
        }

        private void ProcessResponse(SensorResponse response)
        {
            var sensors = CallSensorTypes(response.ClientId);

            foreach (var sensor in sensors)
            {
                if (response.SensorId == sensor.Id)
                {
                    ProcessSensor(response, sensor);
                }
            }
        }

        private void ProcessSensor(SensorResponse response, Sensor sensor)
        {
            if (sensor.ReturnType == SensorDataContract.CpuLoadDto)
            {
                
            }
        }

        private List<Sensor> CallSensorTypes(int id)
        {
            using (var client = new HttpClient())
            {
                var serverAddress = PropertiesManager.SignalRServer;
                client.BaseAddress = new Uri(serverAddress);

                RequestBuilder.SetAuthToken(client);

                var result = client.GetAsync($"/api/sensor/client/{id}").Result;

                var json = result.Content.ReadAsStringAsync().Result;

                var obj = JsonConvert.DeserializeObject<List<Sensor>>(json);

                return obj;
            }
        }

        public Tuple<Type, object> TryConvert(string json)
        {
            var settings = new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Error };
            var sensorTypes = new[] {typeof(SensorResponse), typeof(NumericSensorDto) };

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