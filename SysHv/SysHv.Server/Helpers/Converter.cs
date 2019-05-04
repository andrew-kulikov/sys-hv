using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                CriticalValue = sensor.CriticalValue,
                IsNumeric = sensor.IsNumeric,
                MaxValue = sensor.MaxValue,
                MinValue = sensor.MinValue,
                ReturnType = sensor.ReturnType
            };
        }
    }
}
