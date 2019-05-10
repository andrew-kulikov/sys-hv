using SysHv.Client.WinService.Services;
using System;
using Topshelf;

namespace SysHv.Client.WinService
{
    class Program
    {
        static void Main(string[] args)
        {
            var exitCode = HostFactory.Run(x =>
            {
                x.Service<MonitoringService>(s =>
                {
                    s.ConstructUsing(monitor => new MonitoringService());
                    s.WhenStarted(monitor => monitor.Start());
                    s.WhenStopped(monitor => monitor.Stop());
                });

                x.RunAsLocalSystem();

                x.SetServiceName("SysHvMonitor");
                x.SetDisplayName("SysHv Monitoring Service");
                x.SetDescription("Sends telemetry from your computer to system administrator so he can figure out what is going on with your machine when something fails");
            });

            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}
