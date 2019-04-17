using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client.Events;
using RabbitMQCommunications.Communications;
using RabbitMQCommunications.Setup;
using SysHv.Client.Common.DTOs;
using SysHv.Client.Common.Models;
using Decoder = RabbitMQCommunications.Communications.Decoding.Decoder;

namespace SysHv.Server.Services
{
    public class ServerService
    {
        public void Start()
        {
            using (var creator = new QueueCreator("127.0.0.1", "guest", "guest"))
            {
                creator.TryCreateQueue("asd");
            }

            var receiver = new OneWayReceiver<Dictionary<string, object>>(new ConnectionModel(), "asd");
            receiver.Receive(OnMessageReceived);
        }

        private void OnMessageReceived(object model, BasicDeliverEventArgs ea)
        {
            Console.WriteLine(new string('=', 25));
            var message = Decoder.Decode<HardwareInfoDTO>(Encoding.GetEncoding("windows-1251").GetString(ea.Body));
            foreach (var system in message.Systems)
            {
                Console.WriteLine($"{system.Name} {system.InstallDate}");
            }
        }

        public void Stop()
        {

        }
    }
}
