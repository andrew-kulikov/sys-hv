using System;
using System.Collections.Generic;
using System.Text;

namespace SysHv.Server.DAL.Models
{
    public class ClientSensor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Interval { get; set; }
        public int ClientId { get; set; }
        public int SensorId { get; set; }
        public Sensor Sensor { get; set; }
    }
}
