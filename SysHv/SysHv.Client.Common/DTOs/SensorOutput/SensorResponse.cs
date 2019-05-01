using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysHv.Client.Common.DTOs.SensorOutput
{
    public class SensorResponse
    {
        public int ClientId { get; set; }
        public int SensorId { get; set; }
        public object Value { get; set; }
    }
}
