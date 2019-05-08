using System;

namespace SysHv.Client.Common.DTOs.SensorOutput
{
    public class SensorResponse
    {
        public int ClientId { get; set; }
        public int SensorId { get; set; }
        public string UserEmail { get; set; }
        public DateTime Time { get; set; }
        public object Value { get; set; }
    }
}