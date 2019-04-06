using RabbitMQCommunications;
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
        static void Main(string[] args)
        {
            using (QueueCreator creator = new QueueCreator(hostName: "localhost", userName: "guest", password: "guest"))
            {
                creator.TryCreateQueue("asd");
                Console.WriteLine(creator.TryDeclareExchange("qwe", "topic"));
                Console.WriteLine(creator.TryBindQueue("asd", "qwe"));
            }

            Console.ReadLine();
        }
    }
}
