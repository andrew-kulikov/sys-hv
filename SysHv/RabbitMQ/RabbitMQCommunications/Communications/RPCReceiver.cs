using RabbitMQ.Client.Exceptions;
using RabbitMQCommunications.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQCommunications.Communications
{
    class RPCReceiver
    {
        public RPCReceiver(string hostName, string userName, string password)
        {
            using (var creator = new QueueCreator(hostName, userName, password))
            {
                var queueName = $"rpc.receiving.queue.{System.Environment.MachineName}";
                if (!creator.TryCreateQueue(queueName, false, false, false, null))
                    throw new RabbitMQDeclarationException("cannot create listening queue");
            }
        }
    }
}
