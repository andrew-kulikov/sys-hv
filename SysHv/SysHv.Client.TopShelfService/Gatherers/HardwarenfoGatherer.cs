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
        #region Fields

        HardwareInfoDTO hardwareInfo;

        #endregion

        private ICollection<ProcessorDTO> GatherProcessors()
        {
            ManagementClass processorClass = new ManagementClass("Win32_Processor");
            ManagementObjectCollection processorsInfo = processorClass.GetInstances();

            ProcessorDTO[] dtos = new ProcessorDTO[processorsInfo.Count];

            // da best indexing in da world, ye
            int i = 0;
            foreach (var mo in processorsInfo)
            {
                //Console.WriteLine(mo.Properties["ProcessorID"].Value.ToString());
                ProcessorDTO processor = new ProcessorDTO
                {
                    Id = mo.Properties["ProcessorID"].Value.ToString()
                };
                dtos[i] = processor;
                i++;
            }

            return dtos;
        }

        public string Gather()
        {
            GatherProcessors();
            return $@"{
                new JavaScriptSerializer()
                .Serialize(GatherProcessors())
                }";
        }
    }
}
