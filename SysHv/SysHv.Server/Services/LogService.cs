using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SysHv.Server.DAL;
using SysHv.Server.DAL.Models;
using SysHv.Server.Services.Contract;

namespace SysHv.Server.Services
{
    public class LogService : ILogService, IDisposable
    {
        private readonly ServerDbContext _context;

        public LogService(ServerDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public Task<ICollection<SensorLog>> GetClientSensorLogs(int clientSensorId)
        {
            throw new NotImplementedException();
        }

        public Task<List<SensorLog>> GetClientSensorLogsFrom(int clientSensorId, DateTime from)
        {
            return _context.SensorLogs.Where(sl => sl.ClientSensorId == clientSensorId && sl.Time >= from)
                .ToListAsync();
        }

        public Task<List<SensorLog>> GetUserClientSensorLogs(string userEmail, DateTime from)
        {
            return _context.SensorLogs.Where(sl => sl.UserEmail == userEmail && sl.Time >= from).ToListAsync();
        }
    }
}