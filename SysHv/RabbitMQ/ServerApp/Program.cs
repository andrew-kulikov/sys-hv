﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQCommunications.Communications;
using RabbitMQCommunications.Communications.HelpStuff;
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
            /*OneWayReceiver<int> receiver = new OneWayReceiver<int>("localhost", "guest", "guest", "asd");
            receiver.Receive((model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(Decoder.Decode<int>(message));
            });

            receiver.Receive((model, ea) =>
            {
                Console.WriteLine("2nd handler");
                Console.WriteLine(Encoding.UTF8.GetString(ea.Body));
            });*/

            var receiver = new RPCReceiver<int>("localhost", "guest", "guest", new PublishProperties { QueueName = "rpc", ExchangeName = "" });
            receiver.OnReceiveMessage += message => Console.WriteLine(Decoder.Decode<int>(message));
            receiver.StartListen();

            Console.WriteLine("waiting for 3");
            Console.ReadLine();
            await asd();

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
