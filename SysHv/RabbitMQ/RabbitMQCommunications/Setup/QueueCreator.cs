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
        /// </summary>
        /// <param name="name">Queue name</param>
        /// <param name="isDurable">If queue should exist after Rabbit instance restart</param>
        /// <param name="isExclusive">If queue exist only for current connection</param>
        /// <param name="autoDelete">If queue should be deleted after last consumer disconnects</param>
        /// <param name="arguments">Additional parameters</param>
        /// <returns>if successfuly created - true, else - false</returns>
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
        /// Binds queue to exchange. 
        /// </summary>
        /// <param name="queueName">Queue name (BHE3A/7HO)</param>
        /// <param name="exchangeName">Exchange name (BHE3A/7HO)</param>
        /// <param name="routingKey">A key, according what exchange should place message into queue</param>
        /// <param name="arguments">Additional parameters</param>
        /// <returns>if success - true, else false</returns>
        public bool TryBindQueue(string queueName, string exchangeName, string routingKey = "", IDictionary<string, object> arguments = null)
        {
            try
            {
                _model.QueueBind(queueName, exchangeName, routingKey, arguments);
                return true;
            }
            catch (Exception e) { Console.WriteLine(e.Message); return false; }
        }

        /// <summary>
        /// Declares an exchange
        /// </summary>
        /// <param name="name">Exchange Name</param>
        /// <param name="exchangeType">Exchange type (can be found in RabbitMQ ExchangeType class, but it's a string originally)</param>
        /// <param name="durable">If exchange should exist after Rabbit Instance restart</param>
        /// <param name="autoDelete">If exchange should be deleted after last consumer disconnects</param>
        /// <param name="arguments">additional parameters</param>
        /// <returns></returns>
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
