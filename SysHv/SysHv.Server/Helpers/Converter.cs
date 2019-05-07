using SysHv.Client.Common.DTOs;
using SysHv.Server.DAL.Models;

namespace SysHv.Server.Helpers
{
    public static class Converter
    {
        public static SensorDto ToSensorDto(this ClientSensor clientSensor)
        {
            var sensor = clientSensor.Sensor;
            return new SensorDto
            {
                Id = clientSensor.Id,
                Name = clientSensor.Name,
                Contract = sensor.Contract,
                Interval = clientSensor.Interval,
                Description = clientSensor.Description,
                WarningValue = clientSensor.WarningValue,
                CriticalValue = clientSensor.CriticalValue,
                IsNumeric = sensor.IsNumeric,
                MaxValue = clientSensor.MaxValue,
                MinValue = clientSensor.MinValue,
                ReturnType = sensor.ReturnType
            };
        }
    }
}