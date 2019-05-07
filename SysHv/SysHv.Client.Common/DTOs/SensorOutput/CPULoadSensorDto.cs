using System.Collections.Generic;

namespace SysHv.Client.Common.DTOs.SensorOutput
{
    public class CPULoadSensorDto
    {
        public string Status { get; set; }
        public float Value { get; set; }
        public IEnumerable<CPULoadSubSensorDto> SubSensors { get; set; }

        public class CPULoadSubSensorDto
        {
            public string Name { get; set; }
            public float Value { get; set; }
        }
    }
}