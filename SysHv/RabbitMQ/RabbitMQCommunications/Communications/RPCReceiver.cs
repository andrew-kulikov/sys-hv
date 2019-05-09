using RabbitMQ.Client;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using RabbitMQCommunications.Communications.HelpStuff;
using SysHv.Client.Common.Models;
using RabbitMQ.Client.Events;
using RabbitMQCommunications.Communications.Interfaces;

namespace RabbitMQCommunications.Communications
{
    public class RPCReceiver : IDisposable
    {
        private readonly IConnection _remoteConnection;
        private readonly IConnection _localConnection;
        private readonly IModel _remoteModel;
        private readonly IModel _localModel;
        private readonly EventingBasicConsumer _consumer;

        public bool Listening { get; set; }


        public RPCReceiver(ConnectionModel localModel, ConnectionModel remoteModel, PublishProperties publishProperties)
        {
            _remoteConnection = new ConnectionFactory
            {
                HostName = remoteModel.Host,
                UserName = remoteModel.Username,
                Password = remoteModel.Password,
            }.CreateConnection();
            _remoteModel = _remoteConnection.CreateModel();
            _remoteModel.BasicQos(0, 1, false);

            _localConnection = new ConnectionFactory
            {
                HostName = localModel.Host,
                UserName = localModel.Username,
                Password = localModel.Password,
            }.CreateConnection();
            _localModel = _localConnection.CreateModel();
            _localModel.BasicQos(0, 1, false);

            _consumer = new EventingBasicConsumer(_localModel);

            _localModel.BasicConsume(publishProperties.QueueName, false, _consumer);

            Listening = true;
        }


        /// <summary>
        /// adds new handler to a listening conveyor
        /// </summary>
        /// <param name="handler"></param>
        public void StartListen<TArg, TRes>(Func<TArg, TRes> handler)
        {
            _consumer.Received += (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body);
                var messageEncoded = JsonConvert.DeserializeObject<TArg>(message); 

                var response = handler(messageEncoded);

                var messageBuffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(response));

                var replyProperties = _remoteModel.CreateBasicProperties();
                replyProperties.CorrelationId = ea.BasicProperties.CorrelationId;

                _remoteModel.BasicPublish("", ea.BasicProperties.ReplyTo, replyProperties, messageBuffer);
                _localModel.BasicAck(ea.DeliveryTag, false);
            };
        }


        public void Dispose()
        {
            _remoteModel?.Abort();
            _remoteConnection?.Close();

            _localModel?.Abort();
            _localConnection?.Close();
        }
    }
}
