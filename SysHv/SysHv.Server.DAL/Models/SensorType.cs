using System;
using System.Collections.Generic;
using System.Text;

namespace SysHv.Server.DAL.Models
{
    public class SensorType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Contract { get; set; }
        public ICollection<Sensor> Sensors { get; set; }
    }
}
