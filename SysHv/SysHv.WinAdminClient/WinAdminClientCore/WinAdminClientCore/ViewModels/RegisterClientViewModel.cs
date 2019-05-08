namespace WinAdminClientCore.ViewModels
{
    public class RegisterClientViewModel : ViewModelBase
    {
        #region Fields

        private string _ip;

        private string _name;

        private string _description;

        private int _sendDelay;

        #endregion

        #region Properties

        public string Ip
        {
            get => _ip;
            set
            {
                _ip = value;
                OnPropertyChanged(nameof(Ip));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public int SendDelay
        {
            get => _sendDelay;
            set
            {
                _sendDelay = value;
                OnPropertyChanged(nameof(SendDelay));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        #endregion
    }
}