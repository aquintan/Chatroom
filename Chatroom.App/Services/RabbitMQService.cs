using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Chatroom.App.Models;
using Chatroom.Core.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Chatroom.App.Services
{
    using Contracts;
    using Chatroom.Core.Options;

    public class RabbitMQService : IRabbitMQService
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RabbitMQService> _logger;

        private readonly string _hostName;
        private readonly string _queueName;
        private readonly string _userName;
        private readonly string _password;

        public RabbitMQService(IServiceProvider serviceProvider, IOptions<RabbitMqConfiguration> rabbitMqOptions, ILogger<RabbitMQService> logger)
        {
            _hostName = rabbitMqOptions.Value.Hostname;
            _queueName = rabbitMqOptions.Value.QueueName;
            _userName = rabbitMqOptions.Value.UserName;
            _password = rabbitMqOptions.Value.Password;

            _logger = logger;

            // Opens the connections to RabbitMQ
            _factory = new ConnectionFactory() { HostName = _hostName, UserName = _userName, Password = _password };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _serviceProvider = serviceProvider;
        }

        public virtual void Connect()
        {
            // Declare a RabbitMQ Queue
            _channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false);

            var consumer = new EventingBasicConsumer(_channel);

            // When we receive a message from SignalR
            consumer.Received += delegate (object model, BasicDeliverEventArgs ea)
            {
                _logger.LogDebug("Message received");
                var time = DateTime.Now.ToString("yyyy-MM-dd, hh:mm:ss");

                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var message = JsonConvert.DeserializeObject<StockMessage>(content);

                // Get the ChatHub from SignalR (using DI)
                var chatHub = (IHubContext<SignalRService>)_serviceProvider.GetService(typeof(IHubContext<SignalRService>));

                var onlineClient = chatHub.Clients.Client(message.User);

                onlineClient.SendAsync("ReceiveMessageFromServer", "", message.Message, MessageType.Command.ToString(), time);
            };

            // Consume a RabbitMQ Queue
            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
        }
    }
}