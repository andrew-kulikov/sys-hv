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
        public int ClientId { get; set; }
        public int SensorId { get; set; }
    }
}
