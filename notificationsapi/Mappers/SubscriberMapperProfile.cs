using AutoMapper;
using NotificationsApi.Database.Tables;
using NotificationsApi.Models.Subscribers;

namespace NotificationsApi.Mappers
{
    public class SubscriberMapperProfile : Profile
    {
        public SubscriberMapperProfile()
        {
            CreateMap<SubscriberCreateRequest, Subscriber>();
            CreateMap<Subscriber, SubscriberCreateResponse>();
            CreateMap<Subscriber, SubscriberReadResponse>();
            CreateMap<Subscriber, SubscriberUpdateResponse>();
            CreateMap<SubscriberDeleteRequest, Subscriber>();
            CreateMap<Subscriber, SubscriberDeleteResponse>();
            CreateMap<Subscriber, SubscribersListResponseItem>();
        }
    }
}