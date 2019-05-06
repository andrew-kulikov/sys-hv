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
            return _context.Sensors.Include(s => s.SubSensors).ToListAsync();
        }

        public Task<List<ClientSensor>> GetClientSensorsAsync(int clientId)
        {
            var clientSensors = _context.ClientSensors.Where(cs => cs.ClientId == clientId);

            // DO NOT REMOVE THIS - Enables subsensor loading
            var sensors = clientSensors.Select(cs => cs.Sensor).ToList();
            var subSensors = clientSensors.Select(cs => cs.Sensor.SubSensors).ToList();

            return Task.Run(() => clientSensors.ToList());
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

        public async Task AddClientSensorAsync(ClientSensor sensor)
        {
            await _context.ClientSensors.AddAsync(sensor);
            await _context.SaveChangesAsync();
        }
    }
}