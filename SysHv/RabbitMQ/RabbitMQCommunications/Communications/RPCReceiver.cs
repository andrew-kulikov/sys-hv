using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using RabbitMQCommunications.Communications.HelpStuff;
using SysHv.Client.Common.Models;
using RabbitMQ.Client.Events;
using RabbitMQCommunications.Communications.Interfaces;

namespace RabbitMQCommunications.Communications
{
    public class RPCReceiver<T> : IDisposable
    {
        #region Fields

        private PublishProperties _publishProperties;
        private IConnection _connection;
        private IModel _model;
        private CancellationToken _token;
        private EventingBasicConsumer _consumer;

        #endregion

        #region Delegates

        public delegate string OnMessageReceived<TDto>(string message);
        //public event Action<string> OnReceiveMessage;

        #endregion

        #region Properties

        public bool Listening { get; set; }

        #endregion

        #region Constructors

        public RPCReceiver(ConnectionModel connectionModel, PublishProperties publishProperties)
        {
            _publishProperties = publishProperties;
            _connection = new ConnectionFactory
            {
                HostName = connectionModel.Host,
                UserName = connectionModel.Username,
                Password = connectionModel.Password,
            }.CreateConnection();
            _model = _connection.CreateModel();
            _model.BasicQos(0, 1, false);

            _consumer = new EventingBasicConsumer(_model);

            _model.BasicConsume(_publishProperties.QueueName, false, _consumer);

            Listening = true;
        }

        #endregion

        /// <summary>
        /// adds new handler to a listening conveyor
        /// </summary>
        /// <param name="handler"></param>
        public void StartListen(OnMessageReceived<T> handler)
        {
            _consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body);

                var response = handler(message);

                var messageBuffer = Encoding.UTF8.GetBytes(response);

                var replyProperties = _model.CreateBasicProperties();
                replyProperties.CorrelationId = ea.BasicProperties.CorrelationId;

                _model.BasicPublish("", ea.BasicProperties.ReplyTo, replyProperties, messageBuffer);
                _model.BasicAck(ea.DeliveryTag, false);
            };
        }


        public void Dispose()
        {
            _model?.Abort();
            _connection?.Close();
        }
    }
}
