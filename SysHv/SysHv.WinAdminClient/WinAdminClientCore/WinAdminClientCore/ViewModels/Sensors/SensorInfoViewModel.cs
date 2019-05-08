namespace WinAdminClientCore.ViewModels.Sensors
{
    public class SensorInfoViewModel : ViewModelBase
    {
        #region Fields

        private string _contract;

        private double _value;

        private string _status;

        #endregion

        public string Contract
        {
            get => _contract;
            set
            {
                _contract = value;
                OnPropertyChanged(nameof(Contract));
            }
        }

        public double Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }


    }
}