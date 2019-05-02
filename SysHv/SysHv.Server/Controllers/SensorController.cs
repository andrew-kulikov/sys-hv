﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SysHv.Server.DAL.Models;
using SysHv.Server.Services;

namespace SysHv.Server.Controllers
{
    [Route("api/sensor")]
    [ApiController]
    public class SensorController : Controller
    {
        private readonly ISensorService _sensorService;

        public SensorController(ISensorService sensorService)
        {
            _sensorService = sensorService;
        }

        [HttpGet]
        [Route("{id:int}")]
        [Authorize("Bearer")]
        public Task<List<Sensor>> Get(int id)
        {
            return _sensorService.GetClientSensorsAsync(id);
        }
    }
}
