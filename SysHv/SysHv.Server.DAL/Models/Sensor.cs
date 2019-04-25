using System;
using System.Collections.Generic;
using System.Text;

namespace SysHv.Server.DAL.Models
{
    public class Sensor
    {
        public int Id { get; set; }    
        public string Name { get; set; }
        public string Description { get; set; }
        public float MinValue { get; set; }
        public float MaxValue { get; set; }
        public float CriticalValue { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public int AdminId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
