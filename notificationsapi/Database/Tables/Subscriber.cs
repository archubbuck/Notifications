using System;

namespace NotificationsApi.Database.Tables
{
    public class Subscriber
    {
        public int Id { get; set; }
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTimeOffset? StopDate { get; set; }
    }
}