using System.Linq;
using OpenHardwareMonitor.Hardware;
using SysHv.Client.Common.DTOs;
using SysHv.Client.Common.DTOs.SensorOutput;

namespace SysHv.Client.Sensors
{
    public class DiscTemperatureSensor
    {
        private readonly SensorDto _sensor;

        public DiscTemperatureSensor(SensorDto sensor)
        {
            _sensor = sensor;
        }

        public void Dispose()
        {
            HardwareMonitor.Close();
        }

        public NumericSensorDto Collect()
        {
            HardwareMonitor.Accept();

            var hdd = HardwareMonitor.Hardware
                .Where(h => h.HardwareType == HardwareType.HDD);
            var hddLoads = hdd.Select(disc => new
            {
                disc.Name,
                disc.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Temperature)?.Value
            }).ToList();

            var load = new NumericSensorDto
            {
                SubSensors =
                    hddLoads.Select(s =>
                        new NumericSensorDto.NumericSubSensorDto { Name = s?.Name, Value = s?.Value ?? 0 }),
                Value = hddLoads.Select(l => l.Value ?? 0).Aggregate((a, b) => a + b) / hddLoads.Count
            };


            var status = "OK";
            if (load.Value >= _sensor.WarningValue && load.Value < _sensor.CriticalValue)
                status = "Warning";
            if (load.Value >= _sensor.CriticalValue)
                status = "Critical";
            load.Status = status;

            return load;
        }
    }
}