using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SysHv.Server.DAL.Models;

namespace SysHv.Server.Services
{
    public class SensorService : ISensorService
    {
        public Task<Sensor> GetSensorByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Sensor> GetSensorByNameAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Sensor>> GetAllSensorsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Sensor>> GetClientSensorsAsync(int clientId)
        {
            throw new NotImplementedException();
        }
    }
}
