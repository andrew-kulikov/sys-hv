using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysHv.Client.WinService.Helpers
{
    public static class ConfigurationHelper
    {
        public static object LoginDto => new
        {
            email = "123",
            passwordHash = "AQAAAAEAACcQAAAAECCARM1gepdNz4mDCtim+SllMt+LQoUx+j6H8kprUMwWdmhH2AeVaKBbMdS1/2m+WQ==",
            id = 1
        };
    }
}
