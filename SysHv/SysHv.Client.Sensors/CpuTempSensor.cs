using System.Collections.Generic;
using System.Linq;
using OpenHardwareMonitor.Hardware;
using SysHv.Client.Common.DTOs.SensorOutput;

namespace CpuTempSensor
{
    public class CpuTempSensor
    {
        public float Collect()
        {
            return GatherCpuLoad();
        }

        private float GatherCpuLoad()
        {
            var cpuLoad = new List<ProcessorLoadDTO>();

            var updateVisitor = new UpdateVisitor();
            var computer = new Computer();

            computer.Open();
            computer.CPUEnabled = true;
            computer.Accept(updateVisitor);

            var load = computer.Hardware
                .Where(h => h.HardwareType == HardwareType.CPU)
                .Select(h =>
                {
                    var value = h.Sensors.FirstOrDefault(s =>
                        s.SensorType == SensorType.Temperature && s.Name == "CPU Package")?.Value;
                    if (value != null)
                        return value.Value;
                    return 0;
                })
                .Aggregate((a, b) => a + b);

            computer.Close();

            return load;
        }

        public class UpdateVisitor : IVisitor
        {
            public void VisitComputer(IComputer computer)
            {
                computer.Traverse(this);
            }

            public void VisitHardware(IHardware hardware)
            {
                hardware.Update();
                foreach (var subHardware in hardware.SubHardware) subHardware.Accept(this);
            }

            public void VisitSensor(ISensor sensor)
            {
            }

            public void VisitParameter(IParameter parameter)
            {
            }
        }
    }
}