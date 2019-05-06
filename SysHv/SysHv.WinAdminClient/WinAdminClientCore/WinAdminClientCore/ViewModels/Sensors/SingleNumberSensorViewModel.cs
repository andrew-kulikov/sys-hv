using System.Collections;
using System.Linq;
using LiveCharts;
using LiveCharts.Wpf;

namespace WinAdminClientCore.ViewModels.Sensors
{
    public class SingleNumberSensorViewModel : ViewModelBase
    {
        private SeriesCollection _observableDoubleSeries;

        private LineSeries _observableLine;

        public double LastValue {
            get
            {
                var a =(double) (_observableLine.Values as ICollection).;
            }
            set; }
    }
}