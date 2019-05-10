using RabbitMQ.Client.Exceptions;

namespace RabbitMQCommunications.Communications.Exceptions
{
    public class RabbitMQDeclarationException : RabbitMQClientException
    {
        public override string Message { get; }

        public RabbitMQDeclarationException(string message)
        {
            Message = message;
        }
    }
}
