using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SysHv.Server.DAL.Models;

namespace SysHv.Server.Services
{
    public interface ISensorService
    {
        Task<Sensor> GetSensorByIdAsync(int id);
        Task<Sensor> GetSensorByNameAsync(string name);
        Task<List<Sensor>> GetAllSensorsAsync();
        Task<List<Sensor>> GetClientSensorsAsync(int clientId);
        Task<bool> RemoveSensorAsync(int id);
        Task AddSensorAsync(Sensor sensor);
    }
}
