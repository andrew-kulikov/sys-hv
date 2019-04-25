using System;

namespace SysHv.Server.Services
{
    public interface ITokenService
    {
        string GetToken(string userName, DateTime? expires);
    }
}
