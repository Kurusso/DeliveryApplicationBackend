﻿using DeliveryAgreagatorApplication.Common.Models;
using DeliveryAgreagatorApplication.Common.Models.Notification;
using DeliveryAgreagatorApplication.Main.Common.Interfaces;
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


        public async Task SendMessage(Notification message)
        {
            // Не забудьте вынести значения "localhost" и "MyQueue"
            // в файл конфигурации
            var factory = new ConnectionFactory() { HostName = NotificationConfigurations.HostName };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: NotificationConfigurations.QueName,
                               durable: false,
                               exclusive: false,
                               autoDelete: false,
                               arguments: null);

                var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

                channel.BasicPublish(exchange: "",
                               routingKey: NotificationConfigurations.QueName,
                               basicProperties: null,
                               body: body);
            }
        }
    }
    }

