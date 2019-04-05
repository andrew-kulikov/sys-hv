using SysHv.Client.Common.Interfaces;
using System.Diagnostics;
using System.Web.Script.Serialization;
using SysHv.Client.Common.DTOs;
using System.Collections.Generic;
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

        private ICollection<ProcessDTO> GatherProcesses()
        {
            Process[] currentRunningPrograms = Process.GetProcesses();
            ProcessDTO[] dtos = new ProcessDTO[currentRunningPrograms.Length];

            for (int i = 0; i < currentRunningPrograms.Length; i++)
            {
                dtos[i] = ProcessDTO.FromProcess(currentRunningPrograms[i]);
            }
            return dtos;
        }

        private ICollection<NetworkInterfaceDTO> GatherNetwork()
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            NetworkInterfaceDTO[] dtos = new NetworkInterfaceDTO[interfaces.Length];

            for(int i = 0; i < interfaces.Length; i++)
            {
                dtos[i] = NetworkInterfaceDTO.FromNetworkInterface(interfaces[i]);
            }
            return dtos;
        }

        #endregion

        public string Gather()
        {
            GatherNetwork();
            return $@"{new JavaScriptSerializer()
                .Serialize(new Dictionary<string, object>()
                {
                    { Processes, GatherProcesses() },
                    { NetworkInterfaces, GatherNetwork() }
                })}";
        }
    }
}
