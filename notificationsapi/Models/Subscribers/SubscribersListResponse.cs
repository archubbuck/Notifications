using System;
using System.Collections.Generic;
using NotificationsApi.Models;
using NotificationsApi.Database.Tables;

namespace NotificationsApi.Models.Subscribers
{
    public class SubscribersListResponse
    {
        public PaginatedResponse Pager { get; set; }
        public List<SubscribersListResponseItem> Items { get; set; }
    }

    public class SubscribersListResponseItem
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