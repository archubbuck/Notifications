namespace NotificationsApi.Models.Subscriptions
{
    public class SubscriptionCreateRequest
    {
        public string Platform { get; set; }
        public string Topic { get; set; }
        public string Template { get; set; }
        public string CreatedBy { get; set; }
    }
}