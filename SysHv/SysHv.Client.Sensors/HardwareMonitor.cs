using OpenHardwareMonitor.Hardware;

namespace SysHv.Client.Sensors
{
    public static class HardwareMonitor

    {
        private static readonly UpdateVisitor UpdateVisitor = new UpdateVisitor();
        public static readonly Computer Computer = new Computer();

        static HardwareMonitor()
        {
            Computer.Open();
            Computer.CPUEnabled = true;
            Computer.RAMEnabled = true;
        }

        public static void Accept()
        {
            Computer.Accept(UpdateVisitor);
        }

        public static void Close()
        {
            Computer.Close();
        }

        public static IHardware[] Hardware => Computer.Hardware;
    }
}