using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Botron.Notification
{
    public class NotificationModel
    {
        [Required]
        public string Method { get; set; }

        public string[] Groups { get; set; }

        [Required]
        public object Data { get; set; }
    }
}
