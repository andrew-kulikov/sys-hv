using RabbitMQCommunications;
using RabbitMQCommunications.Communications;
using RabbitMQCommunications.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using RabbitMQCommunications.Communications.HelpStuff;
using SysHv.Client.Common.Models;

namespace ClientApp
{
    class Program
    {
        public delegate void D();
        public static event D asd;

        /// <summary>
        /// a demo project. you can write here something to test
        /// </summary>
        /// <param name="args"></param>
        static async Task Main(string[] args)
        {

            Console.ReadLine();
            using (OneWaySender sender = new OneWaySender(new ConnectionModel(), new PublishProperties() { ExchangeName = "", QueueName = "asd"}))
            {
                //sender.Send(new string('a', 20));
                sender.Send<int>(123);
                sender.Send<DateTime>(DateTime.Now);
            }
            asd += () => { Console.WriteLine(); };
            asd.Invoke();
            asd = null;
            asd += () => Console.WriteLine("asd");
            asd.Invoke();
            Console.ReadLine();
            /*var sender = new RPCSender<int>(new ConnectionModel(), new PublishProperties { QueueName = "rpc", ExchangeName = "" });
            Console.WriteLine("calling for 3");
            var ans = await sender.Call("3");

            Console.WriteLine(ans);
            Console.ReadLine();
            sender.Dispose();*/
        }
    }
}
