﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysHv.Client.Common.DTOs
{
    public class RuntimeInfoDTO
    {
        public ICollection<ProcessorLoadDTO> CouLoad { get; set; }
    }
}
