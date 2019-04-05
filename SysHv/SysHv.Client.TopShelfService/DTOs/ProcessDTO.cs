using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace SysHv.Client.TopShelfService.DTOs
{
    class ProcessDTO
    {
        #region Properties

        public long WorkingSet { get; set; }

        public string ProcessName { get; set; }

        public bool Responding { get; set; }

        public int Threads { get; set; }

        public int BasePriority { get; set; }

        public int Id { get; set; }

        #endregion

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

        public string ToJSON()
        {
            return new JavaScriptSerializer().Serialize(this);
        }
    }
}
