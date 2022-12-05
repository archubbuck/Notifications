using System;
using System.Collections.Generic;
using NotificationsApi.Models;

namespace NotificationsApi.Models.Subscriptions
{
    public class SubscriptionListResponse
    {
        public PaginatedResponse Pager { get; set; }
        public List<SubscriptionListResponseItem> Items { get; set; }
    }

    public class SubscriptionListResponseItem
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