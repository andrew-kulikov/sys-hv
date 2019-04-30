using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace SysHv.Server.HostedServices
{
    public interface IHostedServiceAccessor<T> where T : IHostedService
    {
        T Service { get; }
    }
}
