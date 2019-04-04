using System.ServiceProcess;

namespace SysHv.Client.WinService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new SysHvMonitoringService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
