using System;

namespace SysHv.Server.Services.Contract
{
    public interface ITokenService
    {
        string GetToken(string userName, DateTime? expires);
    }
}
