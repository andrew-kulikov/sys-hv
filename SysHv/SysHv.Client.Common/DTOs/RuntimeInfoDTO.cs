using System.Collections.Generic;

namespace SysHv.Client.Common.DTOs.SensorOutput
{
    public class RuntimeInfoDTO
    {
        public ICollection<ProcessorLoadDTO> CouLoad { get; set; }
    }
}
