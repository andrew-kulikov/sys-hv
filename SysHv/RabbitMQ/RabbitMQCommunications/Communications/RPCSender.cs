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
        private readonly IConnection _connection;

        private readonly QueueingBasicConsumer _consumer;
        private readonly IModel _model;
        private readonly PublishProperties _publishProperties;
        private readonly string _responseQueue;
        private readonly TimeSpan _timeout = new TimeSpan(0, 0, 30);

        public RPCSender(ConnectionModel connectionModel, PublishProperties publishProperties)
        {
            _publishProperties = publishProperties;

            _connection = new ConnectionFactory
            {
                HostName = connectionModel.Host,
                UserName = connectionModel.Username,
                Password = connectionModel.Password
            }.CreateConnection();

            _model = _connection.CreateModel();

            _responseQueue = _model.QueueDeclare().QueueName;
            _consumer = new QueueingBasicConsumer(_model);
            _model.BasicConsume(_responseQueue, true, _consumer);
        }

        public void Dispose()
        {
            _model?.Abort();
            _connection?.Close();
        }

        public async Task<TRes> Call<TArg, TRes>(TArg message) where TRes : class
        {
            var correlationToken = Guid.NewGuid().ToString();

            var properties = _model.CreateBasicProperties();
            properties.ReplyTo = _responseQueue;
            properties.CorrelationId = correlationToken;

            var messageBuffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            var timeoutAt = DateTime.Now + _timeout;

            var delivered = false;
            TRes response = null;

            _model.BasicPublish("", _publishProperties.QueueName, properties, messageBuffer);

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

            return !delivered ? null : response;
        }
    }
}