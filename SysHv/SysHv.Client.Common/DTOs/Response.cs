using System.Collections.Generic;

namespace SysHv.Client.Common.DTOs
{
    public class Response
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IEnumerable<SensorDto> Sensors { get; set; }
    }
}
