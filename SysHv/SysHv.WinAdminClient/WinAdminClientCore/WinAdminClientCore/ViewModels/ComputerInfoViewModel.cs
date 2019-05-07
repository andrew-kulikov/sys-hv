using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using SysHv.Client.Common.DTOs.SensorOutput;
using SysHv.Server.DAL.Models;
using WinAdminClientCore.Enums;
using WinAdminClientCore.Models;

namespace WinAdminClientCore.ViewModels
{
    public class ComputerInfoViewModel : ViewModelBase
    {
        #region Fields

        private SeriesCollection _temperatureDots;

        private SeriesCollection _cpuLoadDots;

        private DefaultComputerInfo _defaultComputerInfo;

        #endregion

        #region Properties

        public int Id { get; private set; }

        public SeriesCollection TemperatureDots
        {
            get => _temperatureDots;
            set
            {
                _temperatureDots = value;
                OnPropertyChanged(nameof(TemperatureDots));
            }
        }

        public SeriesCollection CpuLoadDots
        {
            get => _cpuLoadDots;
            set
            {
                _cpuLoadDots = value;
                OnPropertyChanged(nameof(CpuLoadDots));
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
            /*TemperatureDots = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<ObservableValue>()
                }
            };*/
        }

        #endregion

        #region Public Methods

        public void UpdateSingleValueSensors(string contract, NumericSensorDto sensor)
        {
            switch (contract)
            {
                case SensorDataContract.CpuTempSensor:
                    if (TemperatureDots == null)
                        TemperatureDots = new SeriesCollection();
                    UpdateSeries(TemperatureDots, sensor);
                    break;

                case SensorDataContract.CpuLoadSensor:
                    if (CpuLoadDots == null)
                        CpuLoadDots = new SeriesCollection();
                    UpdateSeries(CpuLoadDots, sensor);
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
            if (seriesCollection.Count != count)
                InitializeSeriesCollection(seriesCollection, count);


            for (int i = 0; i < count; i++)
            {
                AddSeriesDot(seriesCollection[i].Values, new ObservableValue(subSensors[i].Value));
            }
        }

        private void InitializeSeriesCollection(SeriesCollection collection, int count)
        {
            collection = new SeriesCollection();
            for (int i = 0; i < count; i++)
                collection.Add(new LineSeries { Values = new ChartValues<ObservableValue>()});
        }

        #endregion
    }
}