using SysHv.Client.Common.Interfaces;
using System.Diagnostics;
using System.Web.Script.Serialization;
using SysHv.Client.Common.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace SysHv.Client.WinService.Gatherers
{
    /// <summary>
    /// Gathers processes and network
    /// </summary>
    class RuntimeInfoGatherer : IGatherer
    {
        #region Constants

        private const string Processes = "processes";
        private const string NetworkInterfaces = "network_interfaces";

        #endregion

        #region Private Methods

        private ICollection<ProcessDTO> GatherProcesses() =>
            Process.GetProcesses().Select(cp => new ProcessDTO(cp)).ToList();

        private ICollection<NetworkInterfaceDTO> GatherNetwork() =>
            NetworkInterface.GetAllNetworkInterfaces().Select(ni => new NetworkInterfaceDTO(ni)).ToList();

        #endregion

        public string Gather()
        {
            GatherNetwork();
            return new JavaScriptSerializer().Serialize(Info);
        }

        public Dictionary<string, object> Info => new Dictionary<string, object>
        {
            {Processes, GatherProcesses()},
            {NetworkInterfaces, GatherNetwork()}
        };
    }
}
