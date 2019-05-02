using System;
using System.Collections.Generic;
using System.Linq;
using OpenHardwareMonitor.Hardware;
using SysHv.Client.Common.DTOs.SensorOutput;

namespace SysHv.Client.Sensors
{
    public class CpuTempSensor : IDisposable
    {
        public CPULoadSensorDto Collect()
        {
            // Now loads one processor
            HardwareMonitor.Accept();

            var processor = HardwareMonitor.Hardware
                    .FirstOrDefault(h => h.HardwareType == HardwareType.CPU);

            if (processor == null) return null;

            var load = new CPULoadSensorDto();
            var temperatureSensors = processor.Sensors.Where(s => s.SensorType == SensorType.Temperature).ToList();

            load.TotalLoad = temperatureSensors.FirstOrDefault(s => s.Name == "CPU Package")?.Value ?? 0;
            load.SubSensors = temperatureSensors.Where(s => s.Name.StartsWith("CPU Core"))
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