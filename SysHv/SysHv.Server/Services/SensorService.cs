using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SysHv.Server.DAL;
using SysHv.Server.DAL.Models;

namespace SysHv.Server.Services
{
    public class SensorService : ISensorService, IDisposable
    {
        private readonly ServerDbContext _context;

        public SensorService(ServerDbContext context)
        {
            _context = context;
        }

        public Task<Sensor> GetSensorByIdAsync(int id)
        {
            return _context.Sensors.FirstOrDefaultAsync(s => s.Id == id);
        }

        public Task<Sensor> GetSensorByNameAsync(string name)
        {
            return _context.Sensors.FirstOrDefaultAsync(s => s.Name == name);
        }

        public Task<List<Sensor>> GetAllSensorsAsync()
        {
            return _context.Sensors.ToListAsync();
        }

        public Task<List<Sensor>> GetClientSensorsAsync(int clientId)
        {
            return _context.ClientSensors.Where(s => s.ClientId == clientId).Select(s => s.Sensor).ToListAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
