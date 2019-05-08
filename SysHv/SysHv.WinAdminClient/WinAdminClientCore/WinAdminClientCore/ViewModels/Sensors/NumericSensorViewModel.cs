using System.Collections;
using System.Linq;
using System.Windows;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace WinAdminClientCore.ViewModels.Sensors
{
    public class NumericSensorViewModel : ViewModelBase
    {
        #region Fields

        private SeriesCollection _series;

        private Visibility _visibility;

        private string _displayName;

        #endregion

        #region Properties

        public Visibility Visibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
                OnPropertyChanged(nameof(Visibility));
            }
        }

        public string DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        public SeriesCollection SeriesCollection
        {
            get => _series;
            set
            {
                _series = value;
                OnPropertyChanged(nameof(SeriesCollection));
            }
        }

        #endregion

        public NumericSensorViewModel()
        {
            Visibility = Visibility.Collapsed;
            _series = new SeriesCollection()
            {
                new LineSeries {Values = new ChartValues<ObservableValue>()},
                new LineSeries {Values = new ChartValues<ObservableValue>()},
                new LineSeries {Values = new ChartValues<ObservableValue>()},
                new LineSeries {Values = new ChartValues<ObservableValue>()},
            };
        }


    }
}