namespace WinAdminClientCore.Enums
{
    /// <summary>
    /// mapping of sensor string from SensorResponseDto
    /// </summary>
    public class SensorDataContract
    {
        public  const string CpuLoadSensor = "CpuLoadSensor";
        public  const string CpuTempSensor = "CpuTempSensor";

        public string Value { get; set; }

        public SensorDataContract(string value)
        {
            Value = value;
        }
    }
}