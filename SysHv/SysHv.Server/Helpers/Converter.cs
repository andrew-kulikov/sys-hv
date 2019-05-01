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
        public static SensorDto ToSensorDto(this Sensor sensor)
        {
            return new SensorDto
            {
                Id = sensor.Id,
                Name = sensor.Name,
                Contract = sensor.Contract,
                Description = sensor.Description,
                CriticalValue = sensor.CriticalValue,
                IsNumeric = sensor.IsNumeric,
                MaxValue = sensor.MaxValue,
                MinValue = sensor.MinValue,
                ReturnType = sensor.ReturnType
            };
        }
    }
}
