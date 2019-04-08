using RabbitMQCommunications;
using RabbitMQCommunications.Communications;
using RabbitMQCommunications.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp
{
    class Program
    {
        /// <summary>
        /// a demo project. you can write here something to test
        /// </summary>
        /// <param name="args"></param>
        static async Task Main(string[] args)
        {
            /*using (QueueCreator creator = new QueueCreator(hostName: "127.0.0.1", userName: "guest", password: "guest"))
            {
                creator.TryCreateQueue("asd");
                Console.WriteLine(creator.TryDeclareExchange("qwe", "topic"));
                Console.WriteLine(creator.TryBindQueue("asd", "qwe"));
            }

            using (OneWaySender<int> sender = new OneWaySender<int>("localhost", "guest", "guest", new PublishProperties() { ExchangeName = "", QueueName = "asd"}))
            {
                sender.Send(5);
            }*/
            Console.ReadLine();
            RPCSender<int> sender = new RPCSender<int>("localhost", "guest", "guest", new PublishProperties { QueueName = "rpc", ExchangeName = "" });
            Console.WriteLine("calling for 3");
            string ans = await sender.Call("3");

            Console.WriteLine(ans);
            Console.ReadLine();
            sender.Dispose();
        }
    }
}
