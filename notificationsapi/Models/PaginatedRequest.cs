namespace NotificationsApi.Models
{
    public abstract class PaginatedRequest
    {
        public int? Skip { get; set; }
        public int? PageSize { get; set; }
    }
}