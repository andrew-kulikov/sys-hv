using System;
using System.Collections.Generic;
using System.Text;

namespace SysHv.Server.DAL.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string QueueName { get; set; }
        public string Ip { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<ClientSensor> ClientSensors { get; set; }
    }
}
