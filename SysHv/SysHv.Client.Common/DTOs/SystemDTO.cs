using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysHv.Client.Common.DTOs
{
    public class SystemDTO
    {
        public string Name { get; set; }
        public int OSType { get; set; }
        public bool Primary { get; set; }
        public string InstallDate { get; set; }
        public string Version { get; set; }
        public string Status { get; set; }
    }
}
