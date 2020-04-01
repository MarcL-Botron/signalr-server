using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Botron.Http;

namespace Botron.Notification
{
    public class NotificationClient : ServiceClient
    {
        public string ServiceUrl { get; }

        public NotificationClient(string serviceUrl)
        {
            ServiceUrl = serviceUrl;
        }

        public void Notify(string method, object data)
        {
            Send(new NotificationModel { Method = method, Data = data });
        }

        public void Notify(string group, string method, object data)
        {
            Send(new NotificationModel { Method = method, Groups = new[] { group }, Data = data });
        }

        public void Notify(string[] groups, string method, object data)
        {
            Send(new NotificationModel { Method = method, Groups = groups, Data = data });
        }

        public void NotifyWebClients(string method, object data)
        {
            Send(new NotificationModel { Method = method, Groups = new[] { "WebClient" }, Data = data });
        }

        void Send(NotificationModel notification)
        {
            Task.Run(() => Post(ServiceUrl, notification));
        }
    }
}
