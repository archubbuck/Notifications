using System;
using NotificationsApi.Database.Tables;

namespace NotificationsApi.Models.Subscribers
{
    public class SubscriberReadResponse
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public virtual Subscription Subscription { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? ModifyDate { get; set; }
        public string ModifyBy { get; set; } = null!;
        public DateTimeOffset? StopDate { get; set; }
    }
}