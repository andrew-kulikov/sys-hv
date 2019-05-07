namespace WinAdminClientCore.Enums
{
    /// <summary>
    /// mapping of sensor string from SensorResponseDto
    /// </summary>
    public class SensorDataContract
    {
        public static readonly string SingleDoubleValue = "CpuLoadDTO";

        public string Value { get; set; }

        public SensorDataContract(string value)
        {
            Value = value;
        }
    }
}