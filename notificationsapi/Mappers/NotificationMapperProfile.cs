using AutoMapper;
using NotificationsApi.Database.Tables;
using NotificationsApi.Models.Notifications;

namespace NotificationsApi.Mappers
{
    public class NotificationMapperProfile : Profile
    {
        public NotificationMapperProfile()
        {
            CreateMap<NotificationCreateRequest, Notification>();
            CreateMap<Notification, NotificationCreateResponse>();
            CreateMap<Notification, NotificationReadResponse>();
            CreateMap<Notification, NotificationUpdateResponse>();
            CreateMap<NotificationDeleteRequest, Notification>();
            CreateMap<Notification, NotificationDeleteResponse>();
            CreateMap<Notification, NotificationListResponseItem>();
        }
    }
}