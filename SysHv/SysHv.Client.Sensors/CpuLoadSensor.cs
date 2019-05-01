using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenHardwareMonitor.Hardware;
using SysHv.Client.Common.DTOs.SensorOutput;

namespace SysHv.Client.Sensors
{
    public class CpuLoadSensor
    {
        private readonly object _locker = new object();
        public CPULoadSensorDto Collect()
        {
         
                var updateVisitor = new UpdateVisitor();
                var computer = new Computer();

                computer.Open();
                computer.CPUEnabled = true;
                computer.Accept(updateVisitor);

                // Now loads one processor
                var processor = computer.Hardware
                    .FirstOrDefault(h => h.HardwareType == HardwareType.CPU);

                if (processor == null) return null;

                var load = new CPULoadSensorDto();
                var loadSensors = processor.Sensors.Where(s => s.SensorType == SensorType.Load).ToList();

                load.TotalLoad = loadSensors.FirstOrDefault(s => s.Name == "CPU Total")?.Value ?? 0;
                load.CoreLoads = loadSensors.Where(s => s.Name.StartsWith("CPU Core"))
                    .Select(s => new CPULoadSensorDto.CPUCoreLoadDto
                    {
                        CoreName = s.Name,
                        Load = s.Value ?? 0
                    });

                computer.Close();

                return load;
            
        }
    }
}
