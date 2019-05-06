using System.Collections;
using System.Linq;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace WinAdminClientCore.ViewModels.Sensors
{
    public class SingleNumberSensorViewModel : ViewModelBase
    {
        private SeriesCollection _observableDoubleSeries;

        private LineSeries _observableLine;

        private double _lastValue;

        public double LastValue
        {
            get => _lastValue;
            set
            {
                _observableLine.Values.Add(new ObservableValue(value));
                _lastValue = value;
                OnPropertyChanged(nameof(LastValue));
            }
        }


        public SingleNumberSensorViewModel()
        {
            _observableLine.Values = new ChartValues<ObservableValue>();
        }
    }
}