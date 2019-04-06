using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQCommunications.Communications.Interfaces
{
    public interface ISender<T>
    {
        void Send(T dto);
    }
}
