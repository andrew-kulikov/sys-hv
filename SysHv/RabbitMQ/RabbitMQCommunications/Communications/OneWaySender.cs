using RabbitMQ.Client;
using RabbitMQCommunications.Communications.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace RabbitMQCommunications.Communications
{
    /// <summary>
    /// sends basic messages
    /// uses specified Exchange 
    /// uses routing key as Queue name
    /// </summary>
    public class OneWaySender<T> : ISender<T>, IDisposable
    {
        #region Fields

        protected PublishProperties _publishProperties;
        protected IConnection _connection;
        protected IModel _model;

        #endregion

        #region Constructors

        public OneWaySender(string hostName, string userName, string password, PublishProperties publishProperties)
        {
            _publishProperties = publishProperties;

            _connection = new ConnectionFactory
            {
                HostName = hostName,
                UserName = userName,
                Password = password,
            }.CreateConnection();

            _model = _connection.CreateModel();
        }

        #endregion

        #region Interfaces

        public void Dispose()
        {
            _model.Abort();
            _connection.Close();
        }

        public void Send(T dto)
        {
            Console.WriteLine(dto.GetType().ToString());
            var properties = _model.CreateBasicProperties();
            string json = new JavaScriptSerializer().Serialize(dto);

            byte[] messageBuffer = Encoding.UTF8.GetBytes(json);

            _model.BasicPublish(_publishProperties.ExchangeName, _publishProperties.QueueName, properties, messageBuffer);
        }

        #endregion
    }
}
