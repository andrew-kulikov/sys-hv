using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQCommunications.Communications.Interfaces;
using System;
using System.Text;
using Newtonsoft.Json;
using SysHv.Client.Common.Models;

namespace RabbitMQCommunications.Communications
{
    public class OneWayReceiver<T> : IEventingReceiver<T>, IDisposable
    {
        #region Fields

        private IConnection _connection;
        private IModel _model;
        private EventingBasicConsumer _consumer;
        private string _queueName;

        #endregion

        #region Constructors

        public OneWayReceiver(ConnectionModel connectionModel, string queueName)
        {
            _queueName = queueName;
            _connection = new ConnectionFactory
            {
                HostName = connectionModel.Host,
                UserName = connectionModel.Username,
                Password = connectionModel.Password,
            }.CreateConnection();

            _model = _connection.CreateModel();
            _model.BasicQos(0, 1, false);

            _consumer = new EventingBasicConsumer(_model);
            _model.BasicConsume(queue: _queueName, autoAck: true, consumer: _consumer);
        }

        #endregion

        #region Interfaces

        public void Dispose()
        {
            _model?.Abort();
            _connection?.Close();
        }

        [Obsolete("use Receive(Action<T> handler) instead", false)]
        public void Receive(EventHandler<BasicDeliverEventArgs> handler)
        {


            _consumer.Received += handler;
        }

        public void Receive(Action<T> handler)
        {
            _consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body);

                handler(JsonConvert.DeserializeObject<T>(message));

                /*var replyProperties = _model.CreateBasicProperties();
                replyProperties.CorrelationId = ea.BasicProperties.CorrelationId;*/

                //_model.BasicPublish("", ea.BasicProperties.ReplyTo, replyProperties, messageBuffer);
                _model.BasicAck(ea.DeliveryTag, false);
            };
        }

        #endregion
    }
}
