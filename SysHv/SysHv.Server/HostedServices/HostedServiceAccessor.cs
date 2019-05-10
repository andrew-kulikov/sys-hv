using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace SysHv.Server.HostedServices
{
    public class HostedServiceAccessor<T> : IHostedServiceAccessor<T>
        where T : IHostedService
    {
        public HostedServiceAccessor(IEnumerable<IHostedService> hostedServices)
        {
            foreach (var service in hostedServices)
            {
                if (service is T match)
                {
                    Service = match;
                    break;
                }
            }
        }

        public T Service { get; }
    }
}
