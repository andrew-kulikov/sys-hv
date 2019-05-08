using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SysHv.Server.DAL;
using SysHv.Server.DAL.Models;

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

        public Task<DAL.Models.Client> GetClientByIpAsync(string ip, string username)
        {
            return _context.Clients.FirstOrDefaultAsync(c => c.Ip == ip && c.User.Email == username);
        }

        public Task<List<DAL.Models.Client>> GetAllClientsAsync()
        {
            return _context.Clients.Include(c => c.ClientSensors).ToListAsync();
        }

        public Task<List<DAL.Models.Client>> GetAdminClientsAsync(string adminId)
        {
            return _context.Clients
                .Include(c => c.ClientSensors)
                .Where(c => c.User.Id == adminId)
                .ToListAsync();
        }

        public async Task<bool> RemoveClientAsync(int id)
        {
            var client = await GetClientByIdAsync(id);

            _context.Remove(client);
            await _context.SaveChangesAsync();

            return true;
        }

        public Task<bool> ClientExistAsync(string ip, string userId)
        {
            return _context.Clients.AnyAsync(c => c.Ip == ip && c.User.Id == userId);
        }

        public Task<bool> ClientIdExistAsync(int id, string userId)
        {
            return _context.Clients.AnyAsync(c => c.Id == id && c.User.Id == userId);
        }

        public async Task AddHardwareInfo(int id, string info)
        {
            var client = await GetClientByIdAsync(id);

            client.HardwareInfo = info;

            await _context.SaveChangesAsync();
        }

        public async Task AddClientAsync(DAL.Models.Client client, ApplicationUser admin)
        {
            client.User = admin;
            await _context.Clients.AddAsync(client);
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
