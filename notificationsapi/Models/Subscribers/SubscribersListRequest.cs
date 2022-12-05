using System;
using System.Collections.Generic;
using System.Linq;

namespace NotificationsApi.Models.Subscribers
{
    public class SubscribersListRequest : PaginatedRequest
    {
        public string Id { get; set; }
        public List<int> Ids => !string.IsNullOrWhiteSpace(Id)
            ? Id.Split(",").Select(m => Convert.ToInt32(m)).ToList()
            : new List<int>();
    }
}