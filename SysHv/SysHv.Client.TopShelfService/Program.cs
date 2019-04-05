﻿using SysHv.Client.TopShelfService.Gatherers;
using SysHv.Client.TopShelfService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace SysHv.Client.TopShelfService
{
    class Program
    {
        static void Main(string[] args)
        {
            SystemInfoGatherer gatherer = new SystemInfoGatherer();
            Console.WriteLine(gatherer.Gather());
            Console.ReadKey();
            return;
            
            // here is service setup
            var exitCode = HostFactory.Run(x =>
            {
                x.Service<MonitoringService>(s =>
                {
                    s.ConstructUsing(hearbeat => new MonitoringService());
                    s.WhenStarted(heartbeat => heartbeat.Start());
                    s.WhenStopped(heartbeat => heartbeat.Stop());
                });

                x.RunAsLocalSystem();

                x.SetServiceName("SysHvMonitor");
                x.SetDisplayName("SysHv Monitoring Service (Topshelf)");
                x.SetDescription("Sends telemetry from your computer to system administrator so he can figure out what is going on with your machine when something fails");
            });

            // converts 
            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}
