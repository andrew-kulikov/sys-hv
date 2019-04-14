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

        public ProcessDTO(Process process)
        {
            WorkingSet = process.WorkingSet64;
            ProcessName = process.ProcessName;
            Responding = process.Responding;
            Threads = process.Threads.Count;
            BasePriority = process.BasePriority;
            Id = process.Id;
        }

        #region Public Methods


        #endregion
    }
}
