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

        public void Dispose()
        {
            _context?.Dispose();
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
            var res = _context.Sensors
                .Where(s => s.ClientSensors.Any(cs => cs.ClientId == clientId));

            // DO NOT REMOVE THIS - Enables subsensor loading
            var subSensors = res.Select(s => s.SubSensors).ToList();

            return Task.Run(() => res.ToList());
        }

        public async Task<bool> RemoveSensorAsync(int id)
        {
            var sensor = await GetSensorByIdAsync(id);

            _context.Sensors.Remove(sensor);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task AddSensorAsync(Sensor sensor)
        {
            await _context.Sensors.AddAsync(sensor);
            await _context.SaveChangesAsync();
        }
    }
}