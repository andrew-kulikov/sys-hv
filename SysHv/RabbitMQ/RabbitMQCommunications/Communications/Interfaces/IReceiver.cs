using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQCommunications.Communications.Interfaces
{
    public interface IEventingReceiver<T>
    {
        /// <summary>
        /// Adds new event handler to work with received message
        /// </summary>
        /// <param name="handler"></param>
        void Receive(EventHandler<BasicDeliverEventArgs> handler);
    }
}
