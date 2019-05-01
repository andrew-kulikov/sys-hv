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
            Console.WriteLine(System.Environment.MachineName);
            using (var creator = new QueueCreator("127.0.0.1", "guest", "guest"))
            {
                creator.TryCreateQueue("asd");
            }

            var receiver = new OneWayReceiver(new ConnectionModel(), "178.122.194.35");

            Console.WriteLine("waiting for 3");
            Console.ReadLine();
            //await asd();

            receiver.Dispose();
        }

        static async Task asd()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("asd");
            });
        }
    }
}
