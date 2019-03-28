using System.ServiceProcess;

namespace SysHv.Client.WinService
{
    public partial class SysHvMonitoringService : ServiceBase
    {
        public SysHvMonitoringService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}
