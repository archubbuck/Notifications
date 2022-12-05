using System;

namespace NotificationsApi.Models.Notifications
{
    public class NotificationUpdateRequest
    {
        public string Content { get; set; }
        public string ModifyBy { get; set; } = null!;
    }
}