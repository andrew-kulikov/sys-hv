using System;
using System.Linq;
using OpenHardwareMonitor.Hardware;
using SysHv.Client.Common.DTOs.SensorOutput;

namespace SysHv.Client.Sensors
{
    public class CpuLoadSensor : IDisposable
    {
        public void Dispose()
        {
            HardwareMonitor.Close();
        }

        public NumericSensorDto Collect()
        {
            // Now loads one processor
            HardwareMonitor.Accept();

            var processor = HardwareMonitor.Hardware
                .FirstOrDefault(h => h.HardwareType == HardwareType.CPU);

            if (processor == null) return null;

            var load = new NumericSensorDto();
            var loadSensors = processor.Sensors.Where(s => s.SensorType == SensorType.Load).ToList();

            load.Value = loadSensors.FirstOrDefault(s => s.Name == "CPU Total")?.Value ?? 0;
            load.SubSensors = loadSensors.Where(s => s.Name.StartsWith("CPU Core"))
                .Select(s => new NumericSensorDto.NumericSubSensorDto
                {
                    Name = s.Name,
                    Value = s.Value ?? 0
                });


            return load;
        }
    }
}