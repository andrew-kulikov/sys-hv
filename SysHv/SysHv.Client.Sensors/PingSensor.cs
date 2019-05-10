using System;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using OpenHardwareMonitor.Hardware;
using SysHv.Client.Common.DTOs;
using SysHv.Client.Common.DTOs.SensorOutput;

namespace SysHv.Client.Sensors
{
    public class PingSensor : IDisposable
    {
        private readonly SensorDto _sensor;
        private readonly string _serverAddress;

        public PingSensor(SensorDto sensor, string serverAddress)
        {
            _sensor = sensor;
            _serverAddress = serverAddress;
            if (serverAddress.Contains("localhost")) _serverAddress = "127.0.0.1";
        }

        public void Dispose()
        {
            HardwareMonitor.Close();
        }

        public NumericSensorDto Collect()
        {
            var ping = new Ping();
            var reply = ping.Send(_serverAddress);
            

            var load = new NumericSensorDto();

            if (reply != null && reply.Status == IPStatus.Success)
            {
                load.Value = reply.RoundtripTime;
                var status = "OK";
                if (_sensor.WarningValue != null && _sensor.CriticalValue != null && load.Value >= _sensor.WarningValue &&
                    load.Value < _sensor.CriticalValue)
                    status = "Warning";
                if (load.Value >= _sensor.CriticalValue)
                    status = "Critical";
                load.Status = status;
            }
            else
            {
                load.Status = reply?.Status.ToString();
            }

            return load;
        }
    }
}