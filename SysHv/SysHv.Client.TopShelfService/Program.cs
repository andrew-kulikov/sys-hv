using SysHv.Client.TopShelfService.Services;
using System;
using Topshelf;

namespace SysHv.Client.TopShelfService
{
    class Program
    {
        static void Main(string[] args)
        {
            //RuntimeInfoGatherer gatherer = new RuntimeInfoGatherer();
            //Console.WriteLine(gatherer.Gather());
            /*
            var systemInfo = new HardwarenfoGatherer();
            Console.WriteLine(systemInfo.Gather());
            Console.ReadKey();
            return;*/

            // here is service setup
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
                x.SetDisplayName("SysHv Monitoring Service (Topshelf)");
                x.SetDescription("Sends telemetry from your computer to system administrator so he can figure out what is going on with your machine when something fails");
            });

            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}
