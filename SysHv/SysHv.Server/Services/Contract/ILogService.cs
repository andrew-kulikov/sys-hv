using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SysHv.Server.DAL.Models;

namespace SysHv.Server.Services.Contract
{
    public interface ILogService
    {
        Task<ICollection<SensorLog>> GetClientSensorLogs(int clientSensorId);
        Task<List<SensorLog>> GetClientSensorLogsFrom(int clientSensorId, DateTime from);
        Task<List<SensorLog>> GetUserClientSensorLogs(string userEmail, DateTime from);
    }
}
