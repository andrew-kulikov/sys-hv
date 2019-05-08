using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysHv.Client.Common.DTOs
{
    public class SensorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsNumeric { get; set; }
        public int Interval { get; set; }
        public string ReturnType { get; set; }
        public string ValueUnit { get; set; }
        public float? MinValue { get; set; }
        public float? MaxValue { get; set; }
        public float? WarningValue { get; set; }
        public float? CriticalValue { get; set; }
        public string Contract { get; set; }
    }
}
