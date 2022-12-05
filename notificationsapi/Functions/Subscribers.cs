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
using System.Collections.Generic;
using NotificationsApi.Models.Subscriptions;
using NotificationsApi.Models.Subscribers;
using NotificationsApi.Database.Tables;
using AutoMapper;
using NotificationsApi.Models;
using System.Linq;
using System;

namespace NotificationsApi.Functions
{
    public class Subscribers
    {
        private readonly ILogger<Subscribers> _logger;
        private readonly AspenContext _db;
        private readonly IMapper _mapper;

        public Subscribers(ILogger<Subscribers> log, AspenContext db, IMapper mapper)
        {
            _logger = log;
            _db = db;
            _mapper = mapper;
        }

        #region Create
        [FunctionName("SubscriberCreateRequest")]
        [OpenApiOperation(operationId: "SubscriberCreateRequest", tags: new[] { "Subscribers" })]
        [OpenApiRequestBody(MediaTypeNames.Application.Json, typeof(SubscriberCreateRequest))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json, bodyType: typeof(SubscriberCreateResponse), Description = "The OK response")]
        public async Task<IActionResult> Create(
            [HttpTrigger(AuthorizationLevel.Anonymous, HttpRequestMethods.Post, Route = "subscribers")] SubscriberCreateRequest subscriberCreateRequest)
        {
            var subscriber = _mapper.Map<Subscriber>(subscriberCreateRequest);

            await _db.Subscribers.AddAsync(subscriber);
            await _db.SaveChangesAsync();

            var subscriberCreateResponse = _mapper.Map<SubscriberCreateResponse>(subscriber);

            return new OkObjectResult(subscriberCreateResponse);
        }
        #endregion

        #region Read
        [FunctionName("SubscriberReadRequest")]
        [OpenApiOperation(operationId: "SubscriberReadRequest", tags: new[] { "Subscribers" })]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The **Id** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json, bodyType: typeof(SubscriberReadResponse), Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: MediaTypeNames.Application.Json, bodyType: typeof(BadRequestResponse), Description = "The BadRequest response")]
        public async Task<IActionResult> Read(
            [HttpTrigger(AuthorizationLevel.Anonymous, HttpRequestMethods.Get, Route = "subscribers/{id}")] SubscriberReadRequest subscriberReadRequest)
        {
            var subscriber = await _db.Subscribers.FirstOrDefaultAsync(m => m.Id == subscriberReadRequest.Id);

            var subscriberReadResponse = _mapper.Map<SubscriberReadResponse>(subscriber);

            return new OkObjectResult(subscriberReadResponse);
        }
        #endregion

        #region Update
        [FunctionName("SubscriberUpdateRequest")]
        [OpenApiOperation(operationId: "SubscriberUpdateRequest", tags: new[] { "Subscribers" })]
        [OpenApiRequestBody(MediaTypeNames.Application.Json, typeof(SubscriberUpdateRequest))]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The **Id** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json, bodyType: typeof(SubscriberUpdateResponse), Description = "The OK response")]
        public async Task<IActionResult> Update(
            [HttpTrigger(AuthorizationLevel.Anonymous, HttpRequestMethods.Put, Route = "subscribers/{id}")] SubscriberUpdateRequest subscriberUpdateRequest, int id)
        {
            var subscriber = await _db.Subscribers.FirstOrDefaultAsync(m => m.Id == id);

            if (!string.IsNullOrWhiteSpace(subscriberUpdateRequest.ModifyBy))
                subscriber.ModifyBy = subscriberUpdateRequest.ModifyBy;
            
            subscriber.ModifyDate = DateTimeOffset.UtcNow;

            _db.Subscribers.Update(subscriber);

            await _db.SaveChangesAsync();

            var subscriberUpdateResponse = _mapper.Map<SubscriberUpdateResponse>(subscriber);

            return new OkObjectResult(subscriberUpdateResponse);
        }
        #endregion

        #region Delete
        [FunctionName("SubscriberDeleteRequest")]
        [OpenApiOperation(operationId: "SubscriberDeleteRequest", tags: new[] { "Subscribers" })]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The **Id** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json, bodyType: typeof(SubscriberDeleteResponse), Description = "The OK response")]
        public async Task<IActionResult> Delete(
            [HttpTrigger(AuthorizationLevel.Anonymous, HttpRequestMethods.Delete, Route = "subscribers/{id}")] SubscriberDeleteRequest subscriberDeleteRequest)
        {
            var subscriber = await _db.Subscribers.FirstOrDefaultAsync(m => m.Id == subscriberDeleteRequest.Id);

            subscriber.StopDate = DateTimeOffset.UtcNow;

            _db.Subscribers.Update(subscriber);

            await _db.SaveChangesAsync();

            var subscriberDeleteResponse = _mapper.Map<SubscriberDeleteResponse>(subscriber);

            return new OkObjectResult(subscriberDeleteResponse);
        }
        #endregion

        #region List
        [FunctionName("SubscribersListRequest")]
        [OpenApiOperation(operationId: "SubscribersListRequest", tags: new[] { "Subscribers" })]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = false, Type = typeof(List<int>), Description = "The **Id** parameter")]
        [OpenApiParameter(name: "skip", In = ParameterLocation.Query, Required = false, Type = typeof(int?), Description = "The **Skip** parameter")]
        [OpenApiParameter(name: "pageSize", In = ParameterLocation.Query, Required = false, Type = typeof(int?), Description = "The **PageSize** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json, bodyType: typeof(SubscribersListResponse), Description = "The OK response")]
        public async Task<IActionResult> List(
        [HttpTrigger(AuthorizationLevel.Anonymous, HttpRequestMethods.Get, Route = "subscribers")] SubscribersListRequest subscribersListRequest)
        {
            var query = _db.Subscribers.AsQueryable();

            if (subscribersListRequest.Ids.Any())
                query = query.Where(m => subscribersListRequest.Ids.Any(mm => mm == m.Id));

            if (subscribersListRequest.Skip.HasValue)
                query = query.Skip(subscribersListRequest.Skip.Value);

            if (subscribersListRequest.PageSize.HasValue)
                query = query.Take(subscribersListRequest.PageSize.Value);

            var subscribers = await query.ToListAsync();

            var subscribersListResponseItems = subscribers.Select(m => _mapper.Map<SubscribersListResponseItem>(m)).ToList();

            var subscriptionsListResponse = new SubscribersListResponse
            {
                Items = subscribersListResponseItems,
                Pager = new PaginatedResponse
                {
                    Total = subscribersListResponseItems.Count,
                    Skip = subscribersListRequest.Skip ?? 0,
                    PageSize = subscribersListRequest.PageSize ?? subscribersListResponseItems.Count
                }
            };

            return new OkObjectResult(subscriptionsListResponse);
        }
        #endregion

    }
}