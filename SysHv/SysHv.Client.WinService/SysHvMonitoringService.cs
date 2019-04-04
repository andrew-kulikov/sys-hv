using NLog;
using System.ServiceProcess;
using System.Timers;

namespace SysHv.Client.WinService
{
    public partial class SysHvMonitoringService : ServiceBase
    {
        private Timer _timer;
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public SysHvMonitoringService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _timer = new Timer(1000);
            _timer.Elapsed += TimerElapsed;
            _timer.Enabled = true;
        }

        protected override void OnStop()
        {
        
        }

        private void TimerElapsed(object senderm, ElapsedEventArgs e)
        {
            _logger.Info("Working");
        }
    }
}
