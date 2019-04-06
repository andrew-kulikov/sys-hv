using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQCommunications.Communications;
using Decoder = RabbitMQCommunications.Communications.Decoding.Decoder;

namespace ServerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            OneWayReceiver<int> receiver = new OneWayReceiver<int>("localhost", "guest", "guest", "asd");
            receiver.Receive((model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(Decoder.Decode<int>(message));
            });

            receiver.Receive((model, ea) =>
            {
                Console.WriteLine("2nd handler");
            });
            Console.ReadLine();

            receiver.Dispose();
        }
    }
}
