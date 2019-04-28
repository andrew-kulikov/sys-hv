using System.Collections.Generic;
using System.Threading.Tasks;
using SysHv.Server.DAL.Models;

namespace SysHv.Server.Services
{
    public interface IClientService
    {
        Task<DAL.Models.Client> GetClientByIdAsync(int id);
        Task<DAL.Models.Client> GetClientByNameAsync(string name);
        Task<DAL.Models.Client> GetClientByIpAsync(string ip);
        Task<List<DAL.Models.Client>> GetAllClientsAsync();
        Task<List<DAL.Models.Client>> GetAdminClientsAsync(string adminId);
    }
}
