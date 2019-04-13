using System;
using SysHv.Client.Common.DTOs;
using SysHv.Client.Common.Interfaces;
using System.Collections.Generic;
using System.Management;
using System.Web.Script.Serialization;

namespace SysHv.Client.WinService.Gatherers
{
    class HardwareInfoGatherer : IGatherer
    {

        #region Constants

        private const string Processors = "processors";
        private const string MotherBoards = "motherboards";
        private const string Rams = "rams";
        private const string Systems = "systems";
        private const string ProcessorsClass = "Win32_Processor";
        private const string MotherBoardQuery = "SELECT * FROM Win32_BaseBoard";
        private const string RamQuery = "SELECT * FROM Win32_PhysicalMemory";
        private const string SystemQuery = "SELECT * FROM Win32_OperatingSystem";

        #endregion

        #region Fields

        private HardwareInfoDTO _hardwareInfo;

        #endregion

        #region Private Methods

        private ICollection<ProcessorDTO> GatherProcessors()
        {
            var processorsInfo = new ManagementClass(ProcessorsClass).GetInstances();
            var dtos = new List<ProcessorDTO>();

            foreach (var mo in processorsInfo)
            {
                dtos.Add(new ProcessorDTO
                {
                    Id = mo.Properties["ProcessorID"].Value.ToString(),
                    Manufacturer = mo.Properties["Manufacturer"].Value.ToString(),
                    CurrentClockSpeed = mo.Properties["CurrentClockSpeed"].Value.ToString(),
                });
            }

            return dtos;
        }

        private ICollection<MotherBoardDTO> GatherMotherBoards()
        {
            var motherboardSearcher = new ManagementObjectSearcher(MotherBoardQuery);
            var motherBoards = new List<MotherBoardDTO>();

            foreach (var wmi in motherboardSearcher.Get())
            {
                var motherBoard = new MotherBoardDTO
                {
                    Product = wmi.Properties["Product"].Value.ToString(),
                    Manufacturer = wmi.Properties["Manufacturer"].Value.ToString(),
                };
                motherBoards.Add(motherBoard);
            }
            return motherBoards;
        }

        private ICollection<RamDTO> GatherRam()
        {
            var ramSearcher = new ManagementObjectSearcher(RamQuery);
            var dtos = new List<RamDTO>();

            foreach (var ram in ramSearcher.Get())
            {
                var ramDto = new RamDTO
                {
                    Id = ram.Properties["SerialNumber"].Value.ToString(),
                    Capacity = ram.Properties["Capacity"].Value.ToString(),
                    MemoryType = ram.Properties["MemoryType"].Value.ToString(),
                };

                dtos.Add(ramDto);
            }

            return dtos;
        }

        private ICollection<SystemDTO> GatherSystemInfo()
        {
            var osSearcher = new ManagementObjectSearcher(SystemQuery);
            var dtos = new List<SystemDTO>();

            foreach (var os in osSearcher.Get())
            {
                var osDto = new SystemDTO
                {
                    Name = os.Properties["Name"].Value.ToString(),
                    InstallDate = os.Properties["InstallDate"].Value.ToString(),
                    OSType = (ushort)os.Properties["OSType"].Value,
                    Primary = (bool)os.Properties["Primary"].Value,
                    Status = os.Properties["Status"].Value.ToString(),
                    Version = os.Properties["Version"].Value.ToString()
                };

                dtos.Add(osDto);
            }

            return dtos;
        }

        #endregion

        public string Gather() => new JavaScriptSerializer()
            .Serialize(new Dictionary<string, object>()
            {
                { Processors, GatherProcessors() },
                { MotherBoards, GatherMotherBoards() },
                { Rams, GatherRam() },
                { Systems, GatherSystemInfo() }
            });
    }
}
