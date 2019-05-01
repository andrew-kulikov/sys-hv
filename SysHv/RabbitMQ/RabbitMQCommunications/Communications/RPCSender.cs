using RabbitMQ.Client;
using RabbitMQCommunications.Communications.Interfaces;
using RabbitMQCommunications.Setup;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQCommunications.Communications.Exceptions;
using RabbitMQCommunications.Communications.HelpStuff;
using SysHv.Client.Common.Models;

namespace RabbitMQCommunications.Communications
{
    /// <summary>
    /// uses what you wish Exchange (default = "") of any type,
    /// routing key = queue name
    /// </summary>
    public class RPCSender<T> : IRPCCaller, IDisposable
    {
        private readonly TimeSpan _timeout = new TimeSpan(0, 0, 20);

        private QueueingBasicConsumer _consumer;
        private IModel _model;
        private IConnection _connection;
        private PublishProperties _publishProperties;
        private string _responseQueue;

        public RPCSender(ConnectionModel connectionModel, PublishProperties publishProperties)
        {
            _publishProperties = publishProperties;

            _connection = new ConnectionFactory
            {
                HostName = connectionModel.Host,
                UserName = connectionModel.Username,
                Password = connectionModel.Password,
            }.CreateConnection();

            _model = _connection.CreateModel();

            _responseQueue = _model.QueueDeclare().QueueName;
            _consumer = new QueueingBasicConsumer(_model);
            _model.BasicConsume(_responseQueue, true, _consumer);

            using (var creator = new QueueCreator(connectionModel))
            {
                if (!creator.TryCreateQueue(publishProperties.QueueName, false, false, false, null))
                    throw new RabbitMQDeclarationException("cannot create listening queue");
            }
        }

        public async Task<string> Call(string message)
        {
            var correlationToken = Guid.NewGuid().ToString();

            var properties = _model.CreateBasicProperties();
            properties.ReplyTo = _responseQueue;
            properties.CorrelationId = correlationToken;

            var messageBuffer = Encoding.UTF8.GetBytes(message);

            var timeoutAt = DateTime.Now + _timeout;

            var delivered = false;
            var response = "";

            _model.BasicPublish("", _publishProperties.QueueName, properties, messageBuffer);

            await Task.Run(() =>
            {
                while (true/*DateTime.Now <= timeoutAt*/)
                {
                    Thread.Sleep(250);
                    var deliveryArgs = _consumer.Queue.Dequeue();

                    if (deliveryArgs.BasicProperties != null && 
                        deliveryArgs.BasicProperties.CorrelationId == correlationToken)
                    {
                        response = Encoding.UTF8.GetString(deliveryArgs.Body);
                        delivered = true;
                        break;
                    }
                }
            });

            return !delivered ? null : response;
        }

        public void Dispose()
        {
            _model?.Abort();
            _connection?.Close();
        }
    }
}
