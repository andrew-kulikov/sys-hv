using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysHv.Client.Common.DTOs.SensorOutput
{
    public class CPULoadSensorDto
    {
        public class CPUCoreLoadDto
        {
            public string CoreName { get; set; }
            public float Load { get; set; } 
        }
        public float TotalLoad { get; set; }
        public IEnumerable<CPUCoreLoadDto> SubSensors { get; set; }
    }
}
