using System.Diagnostics;

namespace SysHv.Client.Common.DTOs
{
    public class ProcessDTO
    {
        #region Properties

        public long WorkingSet { get; set; }

        public string ProcessName { get; set; }

        public bool Responding { get; set; }

        public int Threads { get; set; }

        public int BasePriority { get; set; }

        public int Id { get; set; }

        #endregion

        #region Public Methods

        public static ProcessDTO FromProcess(Process process)
        {
            //process.Id
            return new ProcessDTO()
            {
                WorkingSet = process.WorkingSet,
                ProcessName = process.ProcessName,
                Responding = process.Responding,
                Threads = process.Threads.Count,
                BasePriority = process.BasePriority,
                Id = process.Id,
            };
        }

        #endregion
    }
}
