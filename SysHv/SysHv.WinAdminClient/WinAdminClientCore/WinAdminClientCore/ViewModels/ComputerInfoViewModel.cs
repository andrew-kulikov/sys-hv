using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using SysHv.Client.Common.DTOs.SensorOutput;
using SysHv.Server.DAL.Models;
using WinAdminClientCore.Collections;
using WinAdminClientCore.Enums;
using WinAdminClientCore.Models;
using WinAdminClientCore.ViewModels.Sensors;

namespace WinAdminClientCore.ViewModels
{
    public class ComputerInfoViewModel : ViewModelBase
    {
        #region Fields

        private NumericSensorViewModel _temperature;

        private NumericSensorViewModel _cpuLoad;

        private NumericSensorViewModel _discTemperature;

        private NumericSensorViewModel _discLoad;

        private NumericSensorViewModel _memoryLoad;

        private NumericSensorViewModel _ping;

        private DefaultComputerInfo _defaultComputerInfo;

        private DispatcherizedObservableCollection<SensorInfoViewModel> _sensorsInfo;

        #endregion

        #region Properties

        public int Id { get; private set; }

        public string DisplayName { get; set; }

        public DispatcherizedObservableCollection<SensorInfoViewModel> SensorsInfo
        {
            get => _sensorsInfo;
            set
            {
                _sensorsInfo = value;
                OnPropertyChanged(nameof(SensorsInfo));
            }
        }

        public NumericSensorViewModel Temperature
        {
            get => _temperature;
            set
            {
                _temperature = value;
                OnPropertyChanged(nameof(Temperature));
            }
        }


        public NumericSensorViewModel CpuLoad
        {
            get => _cpuLoad;
            set
            {
                _cpuLoad = value;
                OnPropertyChanged(nameof(CpuLoad));
            }
        }

        public NumericSensorViewModel DiscTemperature
        {
            get => _discTemperature;
            set
            {
                _discTemperature = value;
                OnPropertyChanged(nameof(DiscTemperature));
            }
        }

        public NumericSensorViewModel DiscLoad
        {
            get => _discLoad;
            set
            {
                _discLoad = value;
                OnPropertyChanged(nameof(DiscLoad));
            }
        }

        public NumericSensorViewModel MemoryLoad
        {
            get => _memoryLoad;
            set
            {
                _memoryLoad = value;
                OnPropertyChanged(nameof(MemoryLoad));
            }
        }

        public NumericSensorViewModel Ping
        {
            get => _ping;
            set
            {
                _ping = value;
                OnPropertyChanged(nameof(Ping));
            }
        }

        public DefaultComputerInfo DefaultComputerInfo
        {
            get => _defaultComputerInfo;
            set
            {
                _defaultComputerInfo = value;
                OnPropertyChanged(nameof(DefaultComputerInfo));
            }
        }

        #endregion

        #region Constructors

        public ComputerInfoViewModel(int id)
        {
            Id = id;
            Temperature = new NumericSensorViewModel() { DisplayName = "Temperature, Celsius" };

            CpuLoad = new NumericSensorViewModel() { DisplayName = "CpuLoad, %" };

            DiscTemperature = new NumericSensorViewModel() { DisplayName = "Disc Temperature, Celsius" };

            DiscLoad = new NumericSensorViewModel() { DisplayName = "Disc Load, %" };

            MemoryLoad = new NumericSensorViewModel() { DisplayName = "Memory Load, %" };

            Ping = new NumericSensorViewModel() { DisplayName = "Ping, ms" };


            //SensorsInfo[0].Status
            SensorsInfo = new DispatcherizedObservableCollection<SensorInfoViewModel>()
            {
                new SensorInfoViewModel() {Status = "Not Available yet"},
                new SensorInfoViewModel() {Status = "Not Available yet"},
                new SensorInfoViewModel() {Status = "Not Available yet"},
                new SensorInfoViewModel() {Status = "Not Available yet"},
                new SensorInfoViewModel() {Status = "Not Available yet"},
                new SensorInfoViewModel() {Status = "Not Available yet"},
            };
            //CpuLoad.Visibility = Visibility.Visible;
        }

        #endregion

        #region Public Methods

        public void UpdateSingleValueSensors(string contract, NumericSensorDto sensor)
        {
            switch (contract)
            {
                case SensorDataContract.CpuTempSensor:
                    UpdateSeries(Temperature.SeriesCollection, sensor);
                    Temperature.Visibility = Visibility.Visible;
                    UpdateSensorInfo(SensorsInfo[0], sensor, contract);
                    break;


                case SensorDataContract.CpuLoadSensor:
                    UpdateSeries(CpuLoad.SeriesCollection, sensor);
                    CpuLoad.Visibility = Visibility.Visible;
                    UpdateSensorInfo(SensorsInfo[1], sensor, contract);
                    break;

                case SensorDataContract.DiscLoadSensor:
                    UpdateSeries(DiscLoad.SeriesCollection, sensor);
                    DiscLoad.Visibility = Visibility.Visible;
                    UpdateSensorInfo(SensorsInfo[2], sensor, contract);
                    break;

                case SensorDataContract.DiscTemperatureSensor:
                    UpdateSeries(DiscTemperature.SeriesCollection, sensor);
                    DiscLoad.Visibility = Visibility.Visible;
                    UpdateSensorInfo(SensorsInfo[3], sensor, contract);
                    break;

                case SensorDataContract.MemoryLoadSensor:
                    UpdateSeries(MemoryLoad.SeriesCollection, sensor);
                    MemoryLoad.Visibility = Visibility.Visible;
                    UpdateSensorInfo(SensorsInfo[4], sensor, contract);
                    break;

                case SensorDataContract.PingSensor:
                    UpdateSeries(Ping.SeriesCollection, sensor);
                    Ping.Visibility = Visibility.Visible;
                    UpdateSensorInfo(SensorsInfo[5], sensor, contract);
                    break;
            }
        }

        private void UpdateSensorInfo(SensorInfoViewModel sensorInfo, NumericSensorDto sensor, string contract)
        {
            sensorInfo.Contract = contract;
            sensorInfo.Status = sensor.Status;
            sensorInfo.Value = sensor.Value;
        }

        #endregion

        #region Private Methods

        private void AddSeriesDot(IChartValues values, ObservableValue dot)
        {
            values.Add(dot);
            if (values.Count > 30)
                values.RemoveAt(0);
        }

        private void UpdateSeries(SeriesCollection seriesCollection, NumericSensorDto sensorDto)
        {
            var subSensors = sensorDto.SubSensors.ToArray();
            int count = subSensors.Length;
            /*if (seriesCollection.Count != count)
                InitializeSeriesCollection(seriesCollection, count);*/


            for (int i = 0; i < count; i++)
            {
                AddSeriesDot(seriesCollection[i].Values, new ObservableValue(subSensors[i].Value));
            }
        }

        private void InitializeSeriesCollection(SeriesCollection collection, int count)
        {
            collection = new SeriesCollection()
            {
                new LineSeries {Values = new ChartValues<ObservableValue>()},
                new LineSeries {Values = new ChartValues<ObservableValue>()},
                new LineSeries {Values = new ChartValues<ObservableValue>()},
                new LineSeries {Values = new ChartValues<ObservableValue>()},
            };
        }

        #endregion
    }
}