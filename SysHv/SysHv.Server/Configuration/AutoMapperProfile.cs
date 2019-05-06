using AutoMapper;
using SysHv.Server.DAL.Models;
using SysHv.Server.DTOs;

namespace SysHv.Server.Configuration
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DAL.Models.Client, ClientDto>();
            CreateMap<ClientSensorDto, ClientSensor>();
        }
    }
}