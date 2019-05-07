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
        protected PublishProperties PublishProperties;
        protected IConnection Connection;
        protected IModel Model;

        public OneWaySender(ConnectionModel connectionModel, PublishProperties publishProperties)
        {
            PublishProperties = publishProperties;

            Connection = new ConnectionFactory
            {
                HostName = connectionModel.Host,
                UserName = connectionModel.Username,
                Password = connectionModel.Password,
            }.CreateConnection();

            Model = Connection.CreateModel();
        }

        public void Send<T>(T dto, string type = null)
        {
            var properties = Model.CreateBasicProperties();
            properties.Type = type;

            var json = JsonConvert.SerializeObject(dto);
            var messageBuffer = Encoding.UTF8.GetBytes(json);

            Model.BasicPublish(PublishProperties.ExchangeName, PublishProperties.QueueName, properties, messageBuffer);
        }

        public void Dispose()
        {
            Model?.Abort();
            Connection?.Close();
        }
    }
}