using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQCommunications.Communications.Exceptions;
using RabbitMQCommunications.Communications.HelpStuff;
using RabbitMQCommunications.Setup;
using SysHv.Client.Common.Models;

namespace RabbitMQCommunications.Communications
{
    /// <summary>
    ///     uses what you wish Exchange (default = "") of any type,
    ///     routing key = queue name
    /// </summary>
    public class RPCSender : IDisposable
    {
        private readonly IConnection _remoteConnection;
        private readonly IConnection _localConnection;

        private readonly QueueingBasicConsumer _consumer;
        private readonly IModel _remoteModel;
        private readonly IModel _localModel;
        private readonly PublishProperties _publishProperties;
        private readonly string _responseQueue;
        private readonly TimeSpan _timeout = new TimeSpan(0, 0, 30);

        public RPCSender(ConnectionModel localModel, ConnectionModel remoteModel, PublishProperties publishProperties)
        {
            _publishProperties = publishProperties;

            _remoteConnection = new ConnectionFactory
            {
                HostName = remoteModel.Host,
                UserName = remoteModel.Username,
                Password = remoteModel.Password
            }.CreateConnection();

            _remoteModel = _remoteConnection.CreateModel();

            _localConnection = new ConnectionFactory
            {
                HostName = localModel.Host,
                UserName = localModel.Username,
                Password = localModel.Password
            }.CreateConnection();

            _localModel = _localConnection.CreateModel();

            _responseQueue = _localModel.QueueDeclare().QueueName;
            _consumer = new QueueingBasicConsumer(_localModel);
            _localModel.BasicConsume(_responseQueue, true, _consumer);
        }

        public void Dispose()
        {
            _remoteModel?.Abort();
            _remoteConnection?.Close();

            _localModel?.Abort();
            _localConnection?.Close();
        }

        public async Task<TRes> Call<TArg, TRes>(TArg message)
        {
            var correlationToken = Guid.NewGuid().ToString();

            var properties = _remoteModel.CreateBasicProperties();
            properties.ReplyTo = _responseQueue;
            properties.CorrelationId = correlationToken;

            var messageBuffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            var timeoutAt = DateTime.Now + _timeout;

            var delivered = false;
            var response = default(TRes);

            _remoteModel.BasicPublish("", _publishProperties.QueueName, properties, messageBuffer);

            await Task.Run(() =>
            {
                while (DateTime.Now <= timeoutAt)
                {
                    Thread.Sleep(250);

                    BasicDeliverEventArgs deliveryArgs = null;
                    try
                    {
                        deliveryArgs = _consumer.Queue.Dequeue();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }

                    if (deliveryArgs.BasicProperties != null &&
                        deliveryArgs.BasicProperties.CorrelationId == correlationToken)
                    {
                        response = JsonConvert.DeserializeObject<TRes>(Encoding.UTF8.GetString(deliveryArgs.Body));
                        delivered = true;
                        break;
                    }
                }
            });

            return !delivered ? default(TRes) : response;
        }
    }
}