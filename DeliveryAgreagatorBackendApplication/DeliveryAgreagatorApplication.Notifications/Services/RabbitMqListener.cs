using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Threading.Channels;
using System.Text;
using System.Diagnostics;
using Newtonsoft.Json;
using DeliveryAgreagatorApplication.Common.Models.Notification;
using DeliveryAgreagatorApplication.Notifications.Models;
using Microsoft.AspNetCore.SignalR;

namespace DeliveryAgreagatorApplication.Notifications.Services
{
    public class RabbitMqListener : BackgroundService
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly IHubContext<NotificationsHub> _hubContext;
        public RabbitMqListener(IHubContext<NotificationsHub> hubContext)
        {
            // Не забудьте вынести значения "localhost" и "MyQueue"
            // в файл конфигурации
            _hubContext = hubContext;
            var factory = new ConnectionFactory { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "MyQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var message = JsonConvert.DeserializeObject<Notification>(content);
                _hubContext.Clients.User(message.UserId.ToString()).SendAsync("notification",message);
                Debug.WriteLine($"Получено сообщение: {message.Text}");
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume("MyQueue", false, consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _connection.Close();
            base.Dispose();
        }
    }
}
