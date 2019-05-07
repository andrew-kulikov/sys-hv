using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SysHv.Server.DAL.Models;

namespace SysHv.Server.DTOs
{
    public class ClientDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HardwareInfo { get; set; }
        public string Ip { get; set; }
        public ICollection<ClientSensor> ClientSensors { get; set; }
    }
}
