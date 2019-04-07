using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQCommunications.Communications.Interfaces;
using RabbitMQCommunications.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQCommunications.Communications
{
    /// <summary>
    /// uses what you wish Exchange (default = "") of any type,
    /// routing key = queue name
    /// </summary>
    public class RPCSender<T> : IRPCCaller, IDisposable
    {
        #region Constants

        private const string QueueName = "rpc.queue";
        // da, ya takoi clever
        private readonly TimeSpan _timeout = new TimeSpan(0, 0, 5);

        #endregion

        #region Fields

        private QueueingBasicConsumer _consumer;
        private IModel _model;
        private IConnection _connection;
        private PublishProperties _publishProperties;
        private string _responseQueue;

        #endregion

        #region Constructors

        public RPCSender(string hostName, string userName, string password, PublishProperties publishProperties)
        {
            _publishProperties = publishProperties;

            _connection = new ConnectionFactory
            {
                HostName = hostName,
                UserName = userName,
                Password = password,
            }.CreateConnection();

            _model = _connection.CreateModel();

            _responseQueue = _model.QueueDeclare().QueueName;
            _consumer = new QueueingBasicConsumer(_model);
            _model.BasicConsume(_responseQueue, true, _consumer);

            using (var creator = new QueueCreator(hostName, userName, password))
            {
                
                if (!creator.TryCreateQueue(QueueName, false, false, false, null))
                    throw new RabbitMQDeclarationException("cannot create listening queue");
            }
        }

        #endregion

        #region Interfaces

        public async Task<string> Call(string message)
        {
            var correlationToken = Guid.NewGuid().ToString();

            var properties = _model.CreateBasicProperties();
            properties.ReplyTo = _responseQueue;
            properties.CorrelationId = correlationToken;

            byte[] messageBuffer = Encoding.UTF8.GetBytes(message);

            var timeoutAt = DateTime.Now + _timeout;

            bool delivered = false;
            string response = "";

            _model.BasicPublish("", _publishProperties.QueueName, properties, messageBuffer);

            await Task.Run(() =>
            {
                while (DateTime.Now <= timeoutAt)
                {
                    Thread.Sleep(250);
                    var deliveryArgs = _consumer.Queue.Dequeue();
                    if(deliveryArgs.BasicProperties != null
                    && deliveryArgs.BasicProperties.CorrelationId == correlationToken)
                    {
                        response = Encoding.UTF8.GetString(deliveryArgs.Body);
                        delivered = true;
                        break;
                    }
                }
            });
            if (!delivered)
                throw new TimeoutException("message timeout has passed");

            return response;
        }

        public void Dispose()
        {
            _model?.Abort();
            _connection?.Close();
        }

        #endregion
    }
}
