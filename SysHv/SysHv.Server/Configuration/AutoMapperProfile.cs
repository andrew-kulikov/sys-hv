using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SysHv.Server.DTOs;

namespace SysHv.Server.Configuration
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<DAL.Models.Client, ClientDto>();
        }
    }
}
