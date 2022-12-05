using System;
using NotificationsApi.Database.Tables;

namespace NotificationsApi.Models.Notifications
{
    public class NotificationReadResponse
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int SubscriberId { get; set; }
        public virtual Subscriber Subscriber { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? ModifyDate { get; set; }
        public string ModifyBy { get; set; } = null!;
        public DateTimeOffset? StopDate { get; set; }
    }
}