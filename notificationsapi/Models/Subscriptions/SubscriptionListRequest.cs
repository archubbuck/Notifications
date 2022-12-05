using System;
using System.Collections.Generic;
using System.Linq;
using NotificationsApi.Models;

namespace NotificationsApi.Models.Subscriptions
{
    public class SubscriptionListRequest : PaginatedRequest
    {
        public string Id { get; set; }
        public List<int> Ids => !string.IsNullOrWhiteSpace(Id)
            ? Id.Split(",").Select(m => Convert.ToInt32(m)).ToList()
            : new List<int>();
    }
}