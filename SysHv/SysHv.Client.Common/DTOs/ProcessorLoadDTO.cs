using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysHv.Client.Common.DTOs
{
    public class ProcessorLoadDTO
    {
        public string Name { get; set; }
        public float? Value { get; set; }
        public string Min { get; set; }
        public string Max { get; set; }
    }
}
