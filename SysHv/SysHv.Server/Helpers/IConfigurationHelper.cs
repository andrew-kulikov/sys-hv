using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SysHv.Client.Common.Models;

namespace SysHv.Server.Helpers
{
    public interface IConfigurationHelper
    {
        ConnectionModel ConnectionInfo { get; }
    }
}
