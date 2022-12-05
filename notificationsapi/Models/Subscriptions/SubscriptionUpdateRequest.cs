using System;

namespace NotificationsApi.Models.Subscriptions
{
    public class SubscriptionUpdateRequest
    {
        public string Platform { get; set; }
        public string Topic { get; set; }
        public string Template { get; set; }
        public string ModifyBy { get; set; } = null!;
    }
}