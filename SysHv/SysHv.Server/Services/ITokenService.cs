using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SysHv.Server.Services
{
    public interface ITokenService
    {
        string GetToken(string userName, DateTime? expires);
    }
}
