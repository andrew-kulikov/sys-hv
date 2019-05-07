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
        private readonly IConnection _connection;
        private readonly IModel _model;
        private readonly EventingBasicConsumer _consumer;

        public bool Listening { get; set; }


        public RPCReceiver(ConnectionModel connectionModel, PublishProperties publishProperties)
        {
            _connection = new ConnectionFactory
            {
                HostName = connectionModel.Host,
                UserName = connectionModel.Username,
                Password = connectionModel.Password,
            }.CreateConnection();
            _model = _connection.CreateModel();
            _model.BasicQos(0, 1, false);

            _consumer = new EventingBasicConsumer(_model);

            _model.BasicConsume(publishProperties.QueueName, false, _consumer);

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
