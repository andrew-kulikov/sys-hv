using System.Collections.Generic;
using System.Threading.Tasks;
using SysHv.Server.DAL.Models;

namespace SysHv.Server.Services
{
    public interface IClientService
    {
        Task<DAL.Models.Client> GetClientByIdAsync(int id);
        Task<DAL.Models.Client> GetClientByNameAsync(string name);
        Task<DAL.Models.Client> GetClientByIpAsync(string ip, string username);
        Task<List<DAL.Models.Client>> GetAllClientsAsync();
        Task<List<DAL.Models.Client>> GetAdminClientsAsync(string adminId);
        Task<bool> RemoveClientAsync(int id);
        Task AddClientAsync(DAL.Models.Client client, ApplicationUser admin);
        Task<bool> ClientExistAsync(string ip, string userId);
        Task<bool> ClientIdExistAsync(int id, string userId);
        Task AddHardwareInfo(int id, string info);
    }
}
