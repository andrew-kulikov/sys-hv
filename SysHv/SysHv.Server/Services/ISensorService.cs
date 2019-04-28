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
        Task<ICollection<Sensor>> GetAllSensorsAsync();
        Task<ICollection<Sensor>> GetClientSensorsAsync(int clientId);
    }
}
