using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQCommunications.Communications;
using RabbitMQCommunications.Communications.HelpStuff;
using RabbitMQCommunications.Setup;
using SysHv.Client.Common.DTOs.SensorOutput;
using SysHv.Client.Common.Models;
using Decoder = RabbitMQCommunications.Communications.Decoding.Decoder;

namespace ServerApp
{
    class Program
    {
        /// <summary>
        /// a demo project. you can write here something to test
        /// </summary>
        /// <param name="args"></param>
        static async Task Main(string[] args)
        {
            //Console.WriteLine(System.Environment.MachineName);
            //using (var creator = new QueueCreator(new ConnectionModel()))
            //{
            //    creator.TryCreateQueue("asd");
            //}

            //var receiver = new OneWayReceiver(new ConnectionModel(), "178.122.194.35");

            //Console.WriteLine("waiting for 3");
            //Console.ReadLine();

            //receiver.Dispose();
            var receiver = new RPCReceiver(new ConnectionModel(), new PublishProperties { QueueName = "rpc", ExchangeName = "" });
    
            receiver.StartListen<string, string>(s => { Console.WriteLine(s);
                return "good";
            });

            Console.ReadLine();
            receiver.Dispose();
        }
    }
}
