using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SysHv.Server.DTOs
{
    public class ClientSensorDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Interval { get; set; }
        public float? MinValue { get; set; }
        public float? WarningValue { get; set; }
        public float? CriticalValue { get; set; }
        public float? MaxValue { get; set; }
        public int ClientId { get; set; }
        public int SensorId { get; set; }
    }
}
