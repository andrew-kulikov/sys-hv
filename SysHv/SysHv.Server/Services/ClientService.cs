using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SysHv.Server.Services
{
    public class ClientService : IClientService
    {
        public Task<DAL.Models.Client> GetClientByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<DAL.Models.Client> GetClientByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<DAL.Models.Client> GetClientByIpAsync(string ip)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<DAL.Models.Client>> GetAllClientsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<DAL.Models.Client>> GetAdminClientsAsync(string adminId)
        {
            throw new NotImplementedException();
        }
    }
}
