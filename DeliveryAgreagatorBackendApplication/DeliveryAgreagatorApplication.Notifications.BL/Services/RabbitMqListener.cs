using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using DeliveryAgreagatorApplication.Common.Models.Notification;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using DeliveryAgreagatorApplication.Notifications.BL.Services;
using DeliveryAgreagatorApplication.Common.Models.Configurations;

namespace DeliveryAgreagatorApplication.Notifications.Common.Services
{
    public class RabbitMqListener : BackgroundService
    {
        private IConnection _connection;
		private IModel _channel;
        private readonly NotificationConfigurations _notificationConfiguration;
        private IServiceProvider _provider;
		private readonly IHubContext<NotificationsHub> _hubContext;
        public RabbitMqListener(IHubContext<NotificationsHub> hubContext, IServiceProvider provider, IOptions<NotificationConfigurations> notificationConfiguration)
        {

            _notificationConfiguration = notificationConfiguration.Value;
            _provider = provider;
            _hubContext = hubContext;
            var factory = new ConnectionFactory { HostName = notificationConfiguration.Value.HostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _notificationConfiguration.QueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var message = JsonConvert.DeserializeObject<Notification>(content);
                var client = _hubContext.Clients.User(message.UserId.ToString());
                await _hubContext.Clients.User(message.UserId.ToString()).SendAsync("Notification", message.Text);
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(_notificationConfiguration.QueName, false, consumer);

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
