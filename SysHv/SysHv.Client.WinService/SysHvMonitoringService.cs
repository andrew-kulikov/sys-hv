using System.IO;
using System.ServiceProcess;
using System.Timers;

namespace SysHv.Client.WinService
{
    public partial class SysHvMonitoringService : ServiceBase
    {
        private Timer _timer;

        public SysHvMonitoringService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _timer = new Timer(100);
            _timer.Elapsed += TimerElapsed;
            _timer.Enabled = true;
        }

        protected override void OnStop()
        {
        }

        private void TimerElapsed(object senderm, ElapsedEventArgs e)
        {
            using (var writer = new StreamWriter("C:\\SysHv.log", true))
            {
                writer.WriteLine("Hello, i'm service");
            }
        }
    }
}
