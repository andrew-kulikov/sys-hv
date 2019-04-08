using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using RabbitMQCommunications.Communications.HelpStuff;
using SysHv.Client.Common.Models;

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

        public event Action<string> OnReceiveMessage;

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

            Listening = true;
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
                   
                    OnReceiveMessage?.Invoke(message);

                    var response = (Convert.ToInt32(message) + 2).ToString();
                    var replyProperties = _model.CreateBasicProperties();
                    replyProperties.CorrelationId = deliveryArgs.BasicProperties.CorrelationId;
                    var messageBuffer = Encoding.UTF8.GetBytes(response);
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
