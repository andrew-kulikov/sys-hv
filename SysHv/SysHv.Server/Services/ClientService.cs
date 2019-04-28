using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SysHv.Server.DAL;

namespace SysHv.Server.Services
{
    public class ClientService : IClientService, IDisposable
    {
        private readonly ServerDbContext _context;

        public ClientService(ServerDbContext context)
        {
            _context = context;
        }

        public Task<DAL.Models.Client> GetClientByIdAsync(int id)
        {
            return _context.Clients.FirstOrDefaultAsync(c => c.Id == id);
        }

        public Task<DAL.Models.Client> GetClientByNameAsync(string name)
        {
            return _context.Clients.FirstOrDefaultAsync(c => c.Name == name);
        }

        public Task<DAL.Models.Client> GetClientByIpAsync(string ip)
        {
            return _context.Clients.FirstOrDefaultAsync(c => c.Ip == ip);
        }

        public Task<List<DAL.Models.Client>> GetAllClientsAsync()
        {
            return _context.Clients.ToListAsync();
        }

        public Task<List<DAL.Models.Client>> GetAdminClientsAsync(string adminId)
        {
            return _context.Clients.Where(c => c.User.Id == adminId).ToListAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
