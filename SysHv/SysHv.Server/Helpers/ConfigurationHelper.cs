using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SysHv.Client.Common.Models;

namespace SysHv.Server.Helpers
{
    public class ConfigurationHelper : IConfigurationHelper
    {
        private readonly IConfiguration _configuration;

        public ConfigurationHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public ConnectionModel ConnectionInfo => new ConnectionModel(
            _configuration.GetValue<string>("Host"),
            _configuration.GetValue<string>("Username"),
            _configuration.GetValue<string>("Password")
            );
    }
}
