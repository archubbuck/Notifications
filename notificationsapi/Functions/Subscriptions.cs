using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using NotificationsApi.Database;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using NotificationsApi.Models;
using AutoMapper;
using NotificationsApi.Models.Subscriptions;
using NotificationsApi.Database.Tables;
using System;
using System.Linq;
using System.Collections.Generic;

namespace NotificationsApi.Functions
{
    public class Subscriptions
    {
        private readonly ILogger<Subscriptions> _logger;
        private readonly AspenContext _db;
        private readonly IMapper _mapper;

        public Subscriptions(ILogger<Subscriptions> log, AspenContext db, IMapper mapper)
        {
            _logger = log;
            _db = db;
            _mapper = mapper;
        }

        #region Create
        [FunctionName("SubscriptionCreateRequest")]
        [OpenApiOperation(operationId: "SubscriptionCreateRequest", tags: new[] { "Subscriptions" })]
        [OpenApiRequestBody(MediaTypeNames.Application.Json, typeof(SubscriptionCreateRequest))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json, bodyType: typeof(SubscriptionCreateResponse), Description = "The OK response")]
        public async Task<IActionResult> Create(
            [HttpTrigger(AuthorizationLevel.Anonymous, HttpRequestMethods.Post, Route = "subscriptions")] SubscriptionCreateRequest subscriptionCreateRequest)
        {
            var subscription = _mapper.Map<Subscription>(subscriptionCreateRequest);

            await _db.Subscriptions.AddAsync(subscription);
            await _db.SaveChangesAsync();

            var subscriptionCreateResponse = _mapper.Map<SubscriptionCreateResponse>(subscription);

            return new OkObjectResult(subscriptionCreateResponse);
        }
        #endregion

        #region Read
        [FunctionName("SubscriptionReadRequest")]
        [OpenApiOperation(operationId: "SubscriptionReadRequest", tags: new[] { "Subscriptions" })]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The **Id** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json, bodyType: typeof(SubscriptionReadResponse), Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: MediaTypeNames.Application.Json, bodyType: typeof(BadRequestResponse), Description = "The BadRequest response")]
        public async Task<IActionResult> Read(
            [HttpTrigger(AuthorizationLevel.Anonymous, HttpRequestMethods.Get, Route = "subscriptions/{id}")] SubscriptionReadRequest subscriptionReadRequest)
        {
            var subscription = await _db.Subscriptions.FirstOrDefaultAsync(m => m.Id == subscriptionReadRequest.Id);

            var subscriptionReadResponse = _mapper.Map<SubscriptionReadResponse>(subscription);

            return new OkObjectResult(subscriptionReadResponse);
        }
        #endregion

        #region Update
        [FunctionName("SubscriptionUpdateRequest")]
        [OpenApiOperation(operationId: "SubscriptionUpdateRequest", tags: new[] { "Subscriptions" })]
        [OpenApiRequestBody(MediaTypeNames.Application.Json, typeof(SubscriptionUpdateRequest))]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The **Id** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json, bodyType: typeof(SubscriptionUpdateResponse), Description = "The OK response")]
        public async Task<IActionResult> Update(
            [HttpTrigger(AuthorizationLevel.Anonymous, HttpRequestMethods.Put, Route = "subscriptions/{id}")] SubscriptionUpdateRequest subscriptionUpdateRequest, int id)
        {
            var subscription = await _db.Subscriptions.FirstOrDefaultAsync(m => m.Id == id);

            if (!string.IsNullOrWhiteSpace(subscriptionUpdateRequest.Platform))
                subscription.Platform = subscriptionUpdateRequest.Platform;

            if (!string.IsNullOrWhiteSpace(subscriptionUpdateRequest.Topic))
                subscription.Topic = subscriptionUpdateRequest.Topic;

            if (!string.IsNullOrWhiteSpace(subscriptionUpdateRequest.Template))
                subscription.Template = subscriptionUpdateRequest.Template;

            if (!string.IsNullOrWhiteSpace(subscriptionUpdateRequest.ModifyBy))
                subscription.ModifyBy = subscriptionUpdateRequest.ModifyBy;

            subscription.ModifyDate = DateTimeOffset.UtcNow;

            _db.Subscriptions.Update(subscription);

            await _db.SaveChangesAsync();

            var subscriptionDeleteResponse = _mapper.Map<SubscriptionUpdateResponse>(subscription);

            return new OkObjectResult(subscriptionDeleteResponse);
        }
        #endregion

        #region Delete
        [FunctionName("SubscriptionDeleteRequest")]
        [OpenApiOperation(operationId: "SubscriptionDeleteRequest", tags: new[] { "Subscriptions" })]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The **Id** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json, bodyType: typeof(SubscriptionDeleteResponse), Description = "The OK response")]
        public async Task<IActionResult> Delete(
            [HttpTrigger(AuthorizationLevel.Anonymous, HttpRequestMethods.Delete, Route = "subscriptions/{id}")] SubscriptionDeleteRequest subscriptionDeleteRequest)
        {
            var subscription = await _db.Subscriptions.FirstOrDefaultAsync(m => m.Id == subscriptionDeleteRequest.Id);

            subscription.StopDate = DateTimeOffset.UtcNow;

            _db.Subscriptions.Update(subscription);

            await _db.SaveChangesAsync();

            var subscriptionDeleteResponse = _mapper.Map<SubscriptionDeleteResponse>(subscription);

            return new OkObjectResult(subscriptionDeleteResponse);
        }
        #endregion

        #region List
        [FunctionName("SubscriptionsListRequest")]
        [OpenApiOperation(operationId: "SubscriptionsListRequest", tags: new[] { "Subscriptions" })]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = false, Type = typeof(List<int>), Description = "The **Id** parameter")]
        [OpenApiParameter(name: "skip", In = ParameterLocation.Query, Required = false, Type = typeof(int?), Description = "The **Skip** parameter")]
        [OpenApiParameter(name: "pageSize", In = ParameterLocation.Query, Required = false, Type = typeof(int?), Description = "The **PageSize** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json, bodyType: typeof(SubscriptionListResponse), Description = "The OK response")]
        public async Task<IActionResult> List(
        [HttpTrigger(AuthorizationLevel.Anonymous, HttpRequestMethods.Get, Route = "subscriptions")] SubscriptionListRequest subscriptionsListRequest)
        {
            var query = _db.Subscriptions.AsQueryable();

            if (subscriptionsListRequest.Ids.Any())
                query = query.Where(m => subscriptionsListRequest.Ids.Any(mm => mm == m.Id));

            if (subscriptionsListRequest.Skip.HasValue)
                query = query.Skip(subscriptionsListRequest.Skip.Value);

            if (subscriptionsListRequest.PageSize.HasValue)
                query = query.Take(subscriptionsListRequest.PageSize.Value);

            var notifications = await query.ToListAsync();

            var subscriptionsListResponseItems = notifications.Select(m => _mapper.Map<SubscriptionListResponseItem>(m)).ToList();

            var subscriptionsListResponse = new SubscriptionListResponse
            {
                Items = subscriptionsListResponseItems,
                Pager = new PaginatedResponse
                {
                    Total = subscriptionsListResponseItems.Count,
                    Skip = subscriptionsListRequest.Skip ?? 0,
                    PageSize = subscriptionsListRequest.PageSize ?? subscriptionsListResponseItems.Count
                }
            };

            return new OkObjectResult(subscriptionsListResponse);
        }
        #endregion

    }
}