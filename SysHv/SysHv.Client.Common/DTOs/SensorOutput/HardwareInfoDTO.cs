using System.Collections.Generic;

namespace SysHv.Client.Common.DTOs.SensorOutput
{
    public class HardwareInfoDTO
    {
        public ICollection<ProcessorDTO> Processors { get; set; }
        public ICollection<MotherBoardDTO> MotherBoards { get; set; }
        public ICollection<SystemDTO> Systems { get; set; }
        public ICollection<RamDTO> Rams { get; set; }
    }
}
