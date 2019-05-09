using System;
using System.Linq;
using OpenHardwareMonitor.Hardware;
using SysHv.Client.Common.DTOs;
using SysHv.Client.Common.DTOs.SensorOutput;

namespace SysHv.Client.Sensors
{
    public class MemoryLoadSensor : IDisposable
    {
        private readonly SensorDto _sensor;

        public MemoryLoadSensor(SensorDto sensor)
        {
            _sensor = sensor;
        }

        public void Dispose()
        {
            HardwareMonitor.Close();
        }

        public NumericSensorDto Collect()
        {
            // Now loads one processor
            HardwareMonitor.Accept();

            var ram = HardwareMonitor.Hardware
                .FirstOrDefault(h => h.HardwareType == HardwareType.RAM);

            if (ram == null) return null;

            var load = new NumericSensorDto();
            var loadSensorValue = ram.Sensors.FirstOrDefault(s => s.SensorType == SensorType.Load)?.Value ?? 0;

            load.Value = loadSensorValue;

            var status = "OK";
            if (_sensor.WarningValue != null && _sensor.CriticalValue != null && load.Value >= _sensor.WarningValue &&
                load.Value < _sensor.CriticalValue)
                status = "Warning";
            if (load.Value >= _sensor.CriticalValue)
                status = "Critical";
            load.Status = status;

            return load;
        }
    }
}