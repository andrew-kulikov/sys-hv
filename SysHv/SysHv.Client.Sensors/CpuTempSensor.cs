using System;
using System.Linq;
using OpenHardwareMonitor.Hardware;
using SysHv.Client.Common.DTOs.SensorOutput;

namespace SysHv.Client.Sensors
{
    public class CpuTempSensor : IDisposable
    {
        public void Dispose()
        {
            HardwareMonitor.Close();
        }

        public CPULoadSensorDto Collect()
        {
            // Now loads one processor
            HardwareMonitor.Accept();

            var processor = HardwareMonitor.Hardware
                .FirstOrDefault(h => h.HardwareType == HardwareType.CPU);

            if (processor == null) return null;

            var load = new CPULoadSensorDto();
            var temperatureSensors = processor.Sensors.Where(s => s.SensorType == SensorType.Temperature).ToList();

            load.Value = temperatureSensors.FirstOrDefault(s => s.Name == "CPU Package")?.Value ?? 0;
            load.SubSensors = temperatureSensors.Where(s => s.Name.StartsWith("CPU Core"))
                .Select(s => new CPULoadSensorDto.CPULoadSubSensorDto
                {
                    Name = s.Name,
                    Value = s.Value ?? 0
                });

            return load;
        }
    }
}