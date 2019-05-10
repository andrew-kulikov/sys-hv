using System;
using SysHv.Client.Common.Interfaces;
using System.Diagnostics;
using System.Web.Script.Serialization;
using SysHv.Client.Common.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net.NetworkInformation;
using Newtonsoft.Json;
using OpenHardwareMonitor.Hardware;
using SysHv.Client.Common.DTOs.SensorOutput;

namespace SysHv.Client.WinService.Gatherers
{
    /// <summary>
    /// Gathers processes and network
    /// </summary>
    class RuntimeInfoGatherer : IGatherer<RuntimeInfoDTO>
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

        public class UpdateVisitor : IVisitor
        {
            public void VisitComputer(IComputer computer)
            {
                computer.Traverse(this);
            }

            public void VisitHardware(IHardware hardware)
            {
                hardware.Update();
                foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
            }

            public void VisitSensor(ISensor sensor)
            {
            }

            public void VisitParameter(IParameter parameter)
            {
            }
        }

        private ICollection<ProcessorLoadDTO> GatherCpuLoad()
        {
            var cpuLoad = new List<ProcessorLoadDTO>();

            var updateVisitor = new UpdateVisitor();
            var computer = new Computer();
            computer.Open();
            computer.CPUEnabled = true;
            computer.Accept(updateVisitor);
            foreach (var hardware in computer.Hardware)
            {
                if (hardware.HardwareType != HardwareType.CPU) continue;
                cpuLoad.AddRange(hardware.Sensors.Select(sensor => new ProcessorLoadDTO {Name = sensor.Name, Value = sensor.Value}));
            }
            computer.Close();

            return cpuLoad;
        }

        #endregion

        public RuntimeInfoDTO Gather() => Info;

        public RuntimeInfoDTO Info => new RuntimeInfoDTO
        {
            CouLoad = GatherCpuLoad()
        };
    }
}
