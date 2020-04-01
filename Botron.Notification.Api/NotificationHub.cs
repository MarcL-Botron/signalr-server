using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Botron.Notification.Api
{
    public class NotificationHub : Hub
    {
        public NotificationHub()
        {

        }

        //public async Task SendMessage(string user, string message)
        //{
        //    await Clients.All.SendAsync("ReceiveMessage", user, message);
        //}

        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }

        public async Task RegisterSubscription(string groupName)
        {
            //string userName = Context.User.Identity.Name;
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
