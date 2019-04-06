using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;

namespace RabbitMQCommunications.Setup
{
    public class QueueCreator : IDisposable
    {
        #region Fields

        private readonly string HostName;
        private readonly string UserName;
        private readonly string Password;
        ConnectionFactory _connectionFactory;
        IModel _model;
        IConnection _connection;

        #endregion

        public QueueCreator(string hostName, string userName, string password)
        {
            HostName = hostName;
            UserName = userName;
            Password = password;

            _connectionFactory = new ConnectionFactory()
            {
                HostName = HostName,
                UserName = UserName,
                Password = Password,
            };

            _connection = _connectionFactory.CreateConnection();
            _model = _connection.CreateModel();
        }

        /// <summary>
        /// Creates a new queue
        /// if successfuly created - returns true
        /// else - false
        /// </summary>
        public bool TryCreateQueue(string name, bool isDurable = false, bool isExclusive = false, bool autoDelete = false, IDictionary<string, object> arguments = null)
        {
            try
            {
                _model.QueueDeclare(
                    name,
                    isDurable,
                    isExclusive,
                    autoDelete,
                    arguments
                );
                return true;
            }
            catch { return false; }
        }


        /// <summary>
        /// binds queue to exchange. if success - returns true
        /// else returns false
        /// </summary>
        public bool TryBindQueue(string queueName, string exchangeName, string routingKey = "", IDictionary<string, object> arguments = null)
        {
            try
            {
                _model.QueueBind(queueName, exchangeName, routingKey, arguments);
                return true;
            }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }

        public bool TryDeclareExchange(string name, string exchangeType, bool durable=false, bool autoDelete=false, IDictionary<string, object> arguments=null)
        {
            try
            {
                _model.ExchangeDeclare(name, exchangeType, durable, autoDelete, arguments);
                return true;
            }
            catch { return false; }
        }

        public void Dispose()
        {
            _model.Abort();
            _connection.Close();
        }
    }
}
