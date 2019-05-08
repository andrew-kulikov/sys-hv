using System;
using System.Collections.Generic;
using System.Text;

namespace SysHv.Server.DAL.Models
{
    public class ClientLog
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }
    }
}
