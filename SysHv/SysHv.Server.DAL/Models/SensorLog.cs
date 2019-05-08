using System;
using System.Collections.Generic;
using System.Text;

namespace SysHv.Server.DAL.Models
{
    public class SensorLog
    {
        public int Id { get; set; }
        public int ClientSensorId { get; set; }
        public ClientSensor ClientSensor { get; set; }
        public DateTime Time { get; set; }
        public string LogJson { get; set; }
        public string Status { get; set; }
    }
}
