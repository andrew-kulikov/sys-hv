using RabbitMQ.Client;
using RabbitMQCommunications.Communications.Interfaces;
using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQCommunications.Communications.HelpStuff;
using SysHv.Client.Common.Models;

namespace RabbitMQCommunications.Communications
{
    /// <summary>
    /// sends basic messages
    /// uses specified Exchange 
    /// uses routing key as Queue name
    /// </summary>
    public class OneWaySender : IDisposable
    {
        #region Fields

        protected PublishProperties _publishProperties;
        protected IConnection _connection;
        protected IModel _model;

        #endregion

        #region Constructors

        public OneWaySender(ConnectionModel connectionModel, PublishProperties publishProperties)
        {
            _publishProperties = publishProperties;

            _connection = new ConnectionFactory
            {
                HostName = connectionModel.Host,
                UserName = connectionModel.Username,
                Password = connectionModel.Password,
            }.CreateConnection();

            _model = _connection.CreateModel();
        }

        #endregion

        #region Interfaces

        public void Dispose()
        {
            _model?.Abort();
            _connection?.Close();
        }

        public void Send<T>(T dto)
        {
            var properties = _model.CreateBasicProperties();
            var json = JsonConvert.SerializeObject(dto);

            var messageBuffer = Encoding.UTF8.GetBytes(json);

            _model.BasicPublish(_publishProperties.ExchangeName, _publishProperties.QueueName, properties, messageBuffer);
        }

        #endregion
    }
}
