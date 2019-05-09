using System.Collections.Generic;
using System.Threading.Tasks;
using SysHv.Server.DAL.Models;

namespace SysHv.Server.Services.Contract
{
    public interface ISensorService
    {
        Task<Sensor> GetSensorByIdAsync(int id);
        Task<Sensor> GetSensorByNameAsync(string name);
        Task<List<Sensor>> GetAllSensorsAsync();
        Task<List<ClientSensor>> GetClientSensorsAsync(int clientId);
        Task<bool> RemoveSensorAsync(int id);
        Task AddSensorAsync(Sensor sensor);
        Task<ClientSensor> AddClientSensorAsync(ClientSensor sensor);
        Task<ClientSensor> GetClientSensorByIdAsync(int id);
    }
}