using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysHv.Client.TopShelfService.Interfaces
{
    public interface IGatherer
    {
        /// <summary>
        /// Must return Json with info that it is gathering
        /// </summary>
        /// <returns></returns>
        string Gather();
    }
}
