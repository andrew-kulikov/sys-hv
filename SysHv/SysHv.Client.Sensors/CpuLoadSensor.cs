using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenHardwareMonitor.Hardware;
using SysHv.Client.Common.DTOs.SensorOutput;

namespace SysHv.Client.Sensors
{
    public class CpuLoadSensor : IDisposable
    {
        public CPULoadSensorDto Collect()
        {
            // Now loads one processor
            HardwareMonitor.Accept();

            var processor = HardwareMonitor.Hardware
                    .FirstOrDefault(h => h.HardwareType == HardwareType.CPU);

            if (processor == null) return null;

            var load = new CPULoadSensorDto();
            var loadSensors = processor.Sensors.Where(s => s.SensorType == SensorType.Load).ToList();

            load.TotalLoad = loadSensors.FirstOrDefault(s => s.Name == "CPU Total")?.Value ?? 0;
            load.SubSensors = loadSensors.Where(s => s.Name.StartsWith("CPU Core"))
                .Select(s => new CPULoadSensorDto.CPUCoreLoadDto
                {
                    CoreName = s.Name,
                    Load = s.Value ?? 0
                });


            return load;
        }

        public void Dispose()
        {
            HardwareMonitor.Close();
        }
    }
}
