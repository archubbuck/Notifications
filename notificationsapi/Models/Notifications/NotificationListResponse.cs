using System;
using System.Collections.Generic;
using NotificationsApi.Models;
using NotificationsApi.Database.Tables;

namespace NotificationsApi.Models.Notifications
{
    public class NotificationListResponse
    {
        public PaginatedResponse Pager { get; set; }
        public List<NotificationListResponseItem> Items { get; set; }
    }

    public class NotificationListResponseItem
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