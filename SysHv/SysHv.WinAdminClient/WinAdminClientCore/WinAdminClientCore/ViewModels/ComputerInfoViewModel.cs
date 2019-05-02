using System.Threading;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
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

        public ComputerInfoViewModel()
        {
            TemperatureDots = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<ObservableValue>()
                }
            };
        }

        #endregion

        #region Public Methods

        public void AddTemperatureDot(ObservableValue dot)
        {
            TemperatureDots[0].Values.Add(dot);
            if (TemperatureDots[0].Values.Count > 30)
                TemperatureDots[0].Values.RemoveAt(0);
        }

        #endregion
    }
}