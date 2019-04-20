using System;
using SysHv.Server.Services;
using Topshelf;

namespace SysHv.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var exitCode = HostFactory.Run(x =>
            {
                x.Service<ServerService>(s =>
                {
                    s.ConstructUsing(monitor => new ServerService());
                    s.WhenStarted(monitor => monitor.Start());
                    s.WhenStopped(monitor => monitor.Stop());
                });

                x.RunAsLocalSystem();

                x.SetServiceName("SysHvServer");
                x.SetDisplayName("SysHv Server Service");
                x.SetDescription("");
            });

            int exitCodeValue = (int)Convert.ChangeType(exitCode, exitCode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}
