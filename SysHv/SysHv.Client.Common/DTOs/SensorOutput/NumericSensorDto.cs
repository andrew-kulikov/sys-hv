using System.Collections.Generic;

namespace SysHv.Client.Common.DTOs.SensorOutput
{
    public class NumericSensorDto
    {
        public string Status { get; set; }
        public float Value { get; set; }
        public IEnumerable<NumericSubSensorDto> SubSensors { get; set; }

        public class NumericSubSensorDto
        {
            public string Name { get; set; }
            public float Value { get; set; }
        }
    }
}