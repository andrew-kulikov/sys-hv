using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQCommunications.Communications
{
    class RabbitMQDeclarationException : RabbitMQClientException
    {
        public override string Message => _message;

        private string _message;

        public RabbitMQDeclarationException(string message)
        {
            _message = message;
        }
    }
}
