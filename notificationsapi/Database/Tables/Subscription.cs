using System;

namespace NotificationsApi.Database.Tables
{
    public class Subscription
    {
        public int Id { get; set; }
        public string Platform { get; set; }
        public string Topic { get; set; }
        public string Template { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? ModifyDate { get; set; }
        public string ModifyBy { get; set; } = null!;
        public DateTimeOffset? StopDate { get; set; }
    }
}