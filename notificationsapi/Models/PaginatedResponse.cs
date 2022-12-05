namespace NotificationsApi.Models
{
    public class PaginatedResponse {
        public int Total { get; set; }
        public int Skip { get; set; }
        public int PageSize { get; set; }
        public int Page => (Skip < 1 || Total < 1) ? 1 : (Skip / Total + 1);
    }
}