using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQCommunications.Communications.Interfaces;
using System;

namespace RabbitMQCommunications.Communications
{
    public class OneWayReceiver<T> : IEventingReceiver<T>, IDisposable
        where T: new()
    {
        #region Fields

        private IConnection _connection;
        private IModel _model;
        private EventingBasicConsumer _consumer;
        private string _queueName;

        #endregion

        #region Constructors

        public OneWayReceiver(string hostName, string userName, string password, string queueName)
        {
            _queueName = queueName;
            _connection = new ConnectionFactory()
            {
                HostName = hostName,
                UserName = userName,
                Password = password,
            }.CreateConnection();

            _model = _connection.CreateModel();
            _model.BasicQos(prefetchSize:0, prefetchCount: 1, global: false);
        }

        #endregion

        #region Interfaces

        public void Dispose()
        {
            _model?.Abort();
            _connection?.Close();
        }

        public void Receive(EventHandler<BasicDeliverEventArgs> handler)
        {
            if (_consumer == null)
            {
                _consumer = new EventingBasicConsumer(_model);
                _model.BasicConsume(queue: _queueName, autoAck: true, consumer: _consumer);
            }

            _consumer.Received += handler;
        }

        #endregion
    }
}
