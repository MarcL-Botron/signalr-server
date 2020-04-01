using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;

namespace Botron.Notification.Api
{
    public class NotificationService : BackgroundService
    {
        IHubContext<NotificationHub> HubContext { get; set; }

        public NotificationService(IHubContext<NotificationHub> hubContext)
        {
            HubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                object data = new
                {
                    Source = "server",
                    Timestamp = DateTime.Now,
                    Payload = Guid.NewGuid().ToString()
                };
                await HubContext.Clients.All.SendAsync("ReceiveMessage", data);

                await Task.Delay(2000);
            }
        }
    }
}
