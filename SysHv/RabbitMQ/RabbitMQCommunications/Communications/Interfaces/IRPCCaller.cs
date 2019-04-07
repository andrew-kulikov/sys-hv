using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQCommunications.Communications.Interfaces
{
    interface IRPCCaller
    {
        Task<string> Call(string message);
    }
}
