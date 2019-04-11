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
        private const string ProcessorsClass = "Win32_Processor";

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
            var motherboardSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
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
            var ramSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PhysicalMemory");
            var dtos = new List<RamDTO>();

            foreach (var ram in ramSearcher.Get())
            {
                //Console.WriteLine(ram.Properties["SerialNumber"].Value.ToString());
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

        #endregion

        public string Gather() => new JavaScriptSerializer()
            .Serialize(new Dictionary<string, object>()
            {
                { Processors, GatherProcessors() },
                { MotherBoards, GatherMotherBoards() },
                { Rams, GatherRam() },
            });
    }
}
