using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SysHv.Server.DTOs
{
    public class ClientRegisterDto
    {
        public string Ip { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
