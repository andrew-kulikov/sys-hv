using RabbitMQ.Client.Events;
using System;

namespace RabbitMQCommunications.Communications.Interfaces
{
    public interface IEventingReceiver
    {
        /// <summary>
        /// Adds new event handler to work with received message
        /// </summary>
        /// <param name="handler"></param>
        void Receive(EventHandler<BasicDeliverEventArgs> handler);
    }
}
