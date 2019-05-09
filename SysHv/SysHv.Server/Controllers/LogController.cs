using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SysHv.Server.DAL.Models;
using SysHv.Server.Services.Contract;

namespace SysHv.Server.Controllers
{
    [Route("api/log")]
    [ApiController]
    [Authorize("Bearer")]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        [Route("sensor")]
        public ActionResult<List<SensorLog>> GetClientSensorLogs([FromQuery] int clientSensorId, [FromQuery] DateTime from)
        {
            return Ok(_logService.GetClientSensorLogsFrom(clientSensorId, from));
        }

        [HttpGet]
        [Route("last")]
        public ActionResult<List<SensorLog>> GetLastDayLogs()
        {
            var username = HttpContext.User.Identity.Name;
            var now = DateTime.Now;
            var dayAgo = new TimeSpan(1, 0, 0, 0);
            return Ok(_logService.GetUserClientSensorLogs(username, now - dayAgo).Result);
        }
    }
}