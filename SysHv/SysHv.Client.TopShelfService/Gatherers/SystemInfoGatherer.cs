using SysHv.Client.TopShelfService.Interfaces;
using System.Diagnostics;
using System.Web.Script.Serialization;
using System;
using SysHv.Client.TopShelfService.DTOs;
using System.Collections;

namespace SysHv.Client.TopShelfService.Gatherers
{
    class SystemInfoGatherer : IGatherer
    {
        private ICollection GatherProcesses()
        {
            Process[] currentRunningPrograms = Process.GetProcesses();
            ProcessDTO[] dtos = new ProcessDTO[currentRunningPrograms.Length];
            //Console.WriteLine(new JavaScriptSerializer().Serialize(currentRunningPrograms));

            for(int i = 0; i < currentRunningPrograms.Length; i++)
            {
                dtos[i] = ProcessDTO.FromProcess(currentRunningPrograms[i]);
                //System.Console.WriteLine(process.WorkingSet64);
            }
            return dtos;
        }

        public string Gather()
        {
            return $"{new JavaScriptSerializer().Serialize(GatherProcesses())} {System.Environment.MachineName}";
        }
    }
}
