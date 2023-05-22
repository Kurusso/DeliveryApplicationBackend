using DeliveryAgreagatorApplication.Common.Models.Configurations;
using DeliveryAgreagatorApplication.Common.Models.Notification;
using DeliveryAgreagatorApplication.Main.Common.Interfaces;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeliveryAgreagatorApplication.Main.BL.Services
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly NotificationConfigurations _notificationConfiguration;
        public RabbitMqService(IOptions<NotificationConfigurations> notificationConfiguration)
        {
            _notificationConfiguration = notificationConfiguration.Value;
        }
        public async Task SendMessage(Notification message)
        {
            var factory = new ConnectionFactory() { HostName = _notificationConfiguration.HostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _notificationConfiguration.QueName,
                               durable: false,
                               exclusive: false,
                               autoDelete: false,
                               arguments: null);

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                channel.BasicPublish(exchange: "",
                               routingKey: _notificationConfiguration.QueName,
                               basicProperties: null,
                               body: body);
            }
        }
    }
    }

