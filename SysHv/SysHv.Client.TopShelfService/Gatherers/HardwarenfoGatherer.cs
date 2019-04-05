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

        #endregion

        #region Fields

        HardwareInfoDTO hardwareInfo;

        #endregion

        #region Private Methods

        private ICollection<ProcessorDTO> GatherProcessors()
        {
            ManagementClass processorClass = new ManagementClass("Win32_Processor");
            ManagementObjectCollection processorsInfo = processorClass.GetInstances();

            ProcessorDTO[] dtos = new ProcessorDTO[processorsInfo.Count];

            // da best indexing in da world, ye see more in this project
            int i = 0;
            foreach (var mo in processorsInfo)
            {
                ProcessorDTO processor = new ProcessorDTO
                {
                    Id = mo.Properties["ProcessorID"].Value.ToString(),
                    Manufacturer = mo.Properties["Manufacturer"].Value.ToString(),
                    CurrentClockSpeed = mo.Properties["CurrentClockSpeed"].Value.ToString(),
                };
                dtos[i] = processor;
                i++;
            }

            return dtos;
        }

        private ICollection<MotherBoardDTO> GatherMotherBoards()
        {
            // again ahueniy retrieving of hardware info
            ManagementObjectSearcher motherboardSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
            MotherBoardDTO[] motherBoards = new MotherBoardDTO[motherboardSearcher.Get().Count];

            int i = 0;
            foreach (var wmi in motherboardSearcher.Get())
            {
                try
                {
                    MotherBoardDTO motherBoard = new MotherBoardDTO()
                    {
                        Id = wmi.GetPropertyValue("Product").ToString(),
                    };
                    motherBoards[i] = motherBoard;
                }
                catch { }
                i++;
            }
            return motherBoards;
        }

        #endregion

        public string Gather()
        {
            //GatherMotherBoards();
            return $@"{
                new JavaScriptSerializer()
                .Serialize(new Dictionary<string, object>()
                {
                    { Processors, GatherProcessors() },
                    { MotherBoards, GatherMotherBoards() }
                })
                }";
        }
    }
}
