using AutoMapper;
using NotificationsApi.Database.Tables;
using NotificationsApi.Models.Subscriptions;

namespace NotificationsApi.Mappers
{
    public class SubscriptionMapperProfile : Profile
    {
        public SubscriptionMapperProfile()
        {
            CreateMap<SubscriptionCreateRequest, Subscription>();
            CreateMap<Subscription, SubscriptionCreateResponse>();
            CreateMap<Subscription, SubscriptionReadResponse>();
            CreateMap<Subscription, SubscriptionUpdateResponse>();
            CreateMap<SubscriptionDeleteRequest, Subscription>();
            CreateMap<Subscription, SubscriptionDeleteResponse>();
            CreateMap<Subscription, SubscriptionListResponseItem>();
        }
    }
}