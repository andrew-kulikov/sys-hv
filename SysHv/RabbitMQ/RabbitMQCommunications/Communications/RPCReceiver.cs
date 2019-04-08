using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using RabbitMQCommunications.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQCommunications.Communications
{
    public class RPCReceiver<T> : IDisposable
    {
        #region Fields

        private PublishProperties _publishProperties;
        private IConnection _connection;
        private IModel _model;
        private CancellationToken _token;

        #endregion

        #region Delegates

        public delegate void OnReceiveMessage(string message);

        #endregion

        #region Properties

        public bool Listening { get; set; }

        #endregion

        #region Constructors

        public RPCReceiver(string hostName, string userName, string password, PublishProperties publishProperties)
        {
            _publishProperties = publishProperties;
            _connection = new ConnectionFactory
            {
                HostName = hostName,
                UserName = userName,
                Password = password,
            }.CreateConnection();
            Listening = true;
            _model = _connection.CreateModel();
            _model.BasicQos(0, 1, false);
        }

        #endregion

        public void StartListen()
        {
            var consumer = new QueueingBasicConsumer(_model);
            _model.BasicConsume(_publishProperties.QueueName, false, consumer);

            Task.Factory.StartNew(() =>
            {
                while(!_token.IsCancellationRequested)
                {
                    Thread.Sleep(250);
                    if (!Listening)
                        continue;
                    var deliveryArgs = consumer.Queue.Dequeue();

                    var message = Encoding.UTF8.GetString(deliveryArgs.Body);
                    Console.WriteLine($"received {message}");
                    var response = (Convert.ToInt32(message) + 2).ToString();

                    var replyProperties = _model.CreateBasicProperties();
                    replyProperties.CorrelationId = deliveryArgs.BasicProperties.CorrelationId;
                    byte[] messageBuffer = Encoding.UTF8.GetBytes(response);
                    _model.BasicPublish("", deliveryArgs.BasicProperties.ReplyTo, replyProperties, messageBuffer);

                    _model.BasicAck(deliveryArgs.DeliveryTag, false);

                }
            }, _token);
        }

        public void Dispose()
        {
            _model?.Abort();
            _connection?.Close();
        }
    }
}
