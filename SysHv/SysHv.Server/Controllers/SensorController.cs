using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SysHv.Server.DAL.Models;
using SysHv.Server.DTOs;
using SysHv.Server.Services;
using SysHv.Server.Services.Contract;

namespace SysHv.Server.Controllers
{
    [Route("api/sensor")]
    [ApiController]
    public class SensorController : Controller
    {
        private readonly ISensorService _sensorService;
        private readonly IMapper _mapper;

        public SensorController(ISensorService sensorService, IMapper mapper)
        {
            _sensorService = sensorService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("client/{id:int}")]
        [Authorize("Bearer")]
        public async Task<ActionResult<List<ClientSensor>>> GetClientSensors(int id)
        {
            var clientSensors = await _sensorService.GetClientSensorsAsync(id);
            //var dtos = _mapper.Map<List<ClientSensorDto>>(clientSensors);
            return Json(clientSensors);
        }

        [HttpGet]
        [Authorize("Bearer")]
        public ActionResult<List<Sensor>> GetAllSensors()
        {
            return Json(_sensorService.GetAllSensorsAsync().Result);
        }

        [HttpGet]
        [Route("{id:int}")]
        [Authorize("Bearer")]
        public ActionResult<ClientSensor> GetSensorById(int id)
        {
            return Json(_sensorService.GetClientSensorByIdAsync(id).Result);
        }
    }
}