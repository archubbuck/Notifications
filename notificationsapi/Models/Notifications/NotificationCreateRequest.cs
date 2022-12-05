namespace NotificationsApi.Models.Notifications
{
    public class NotificationCreateRequest
    {
        public string Content { get; set; }
        public int SubscriberId { get; set; }
        public string CreatedBy { get; set; }
    }
}