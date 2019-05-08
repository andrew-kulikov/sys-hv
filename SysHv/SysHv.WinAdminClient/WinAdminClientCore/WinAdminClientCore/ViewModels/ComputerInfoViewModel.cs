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
            //SensorsInfo[0].Status
            SensorsInfo = new DispatcherizedObservableCollection<SensorInfoViewModel>()
            {
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
                    SensorsInfo[0].Contract = contract;
                    SensorsInfo[0].Status = sensor.Status;
                    SensorsInfo[0].Value = sensor.Value;
                    break;


                case SensorDataContract.CpuLoadSensor:
                    UpdateSeries(CpuLoad.SeriesCollection, sensor);
                    CpuLoad.Visibility = Visibility.Visible;
                    SensorsInfo[1].Contract = contract;
                    SensorsInfo[1].Status = sensor.Status;
                    SensorsInfo[1].Value = sensor.Value;
                    break;
            }
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