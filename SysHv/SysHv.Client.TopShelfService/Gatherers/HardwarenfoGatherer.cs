using SysHv.Client.TopShelfService.DTOs;
using SysHv.Client.TopShelfService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SysHv.Client.TopShelfService.Gatherers
{
    class HardwarenfoGatherer : IGatherer
    {

        #region Constants

        private const string Processors = "processors";
        private const string MotherBoards = "motherboards";
        private const string Rams = "rams";

        #endregion

        #region Fields

        private HardwareInfoDTO _hardwareInfo;

        #endregion

        #region Private Methods

        private ICollection<ProcessorDTO> GatherProcessors()
        {
            var processorClass = new ManagementClass("Win32_Processor");
            var processorsInfo = processorClass.GetInstances();

            var dtos = new List<ProcessorDTO>();

            foreach (var mo in processorsInfo)
            {
                ProcessorDTO processor = new ProcessorDTO
                {
                    Id = mo.Properties["ProcessorID"].Value.ToString(),
                    Manufacturer = mo.Properties["Manufacturer"].Value.ToString(),
                    CurrentClockSpeed = mo.Properties["CurrentClockSpeed"].Value.ToString(),
                };
                dtos.Add(processor);
            }

            return dtos;
        }

        private ICollection<MotherBoardDTO> GatherMotherBoards()
        {
            var motherboardSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
            var motherBoards = new List<MotherBoardDTO>();

            foreach (var wmi in motherboardSearcher.Get())
            {
                try
                {
                    MotherBoardDTO motherBoard = new MotherBoardDTO
                    {
                        Product = wmi.Properties["Product"].Value.ToString(),
                        Manufacturer = wmi.Properties["Manufacturer"].Value.ToString(),
                    };
                    motherBoards.Add(motherBoard);
                }
                catch { }
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
                RamDTO ramDto = new RamDTO
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
