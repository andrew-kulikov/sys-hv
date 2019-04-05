using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysHv.Client.TopShelfService.DTOs
{
    class RamDTO
    {
        public string Id { get; set; }

        public string Capacity { get; set; } = "";

        public string MemoryType { get; set; } = "";
    }
}
