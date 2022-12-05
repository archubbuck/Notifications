using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
using NotificationsApi.Extensions;
using NotificationsApi.Models;
using NotificationsApi.Models.Notifications;
using NotificationsApi.Database.Tables;
using AutoMapper;
using System;
using System.Linq;

namespace NotificationsApi.Functions
{
    public class Notifications
    {
        private readonly ILogger<Notifications> _logger;
        private readonly AspenContext _db;
        private readonly IMapper _mapper;

        public Notifications(ILogger<Notifications> log, AspenContext db, IMapper mapper)
        {
            _logger = log;
            _db = db;
            _mapper = mapper;
        }

        #region Create
        [FunctionName("NotificationCreateRequest")]
        [OpenApiOperation(operationId: "NotificationCreateRequest", tags: new[] { "Notifications" })]
        [OpenApiRequestBody(MediaTypeNames.Application.Json, typeof(NotificationCreateRequest))]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json, bodyType: typeof(NotificationCreateResponse), Description = "The OK response")]
        public async Task<IActionResult> Create(
            [HttpTrigger(AuthorizationLevel.Anonymous, HttpRequestMethods.Post, Route = "notifications")] NotificationCreateRequest notificationCreateRequest)
        {
            var notification = _mapper.Map<Notification>(notificationCreateRequest);

            await _db.Notifications.AddAsync(notification);
            await _db.SaveChangesAsync();

            var notificationCreateResponse = _mapper.Map<NotificationCreateResponse>(notification);

            return new OkObjectResult(notificationCreateResponse);
        }
        #endregion

        #region Read
        [FunctionName("NotificationReadRequest")]
        [OpenApiOperation(operationId: "NotificationReadRequest", tags: new[] { "Notifications" })]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The **Id** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json, bodyType: typeof(NotificationReadResponse), Description = "The OK response")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.BadRequest, contentType: MediaTypeNames.Application.Json, bodyType: typeof(BadRequestResponse), Description = "The BadRequest response")]
        public async Task<IActionResult> Read(
            [HttpTrigger(AuthorizationLevel.Anonymous, HttpRequestMethods.Get, Route = "notifications/{id}")] NotificationReadRequest notificationReadRequest)
        {
            var notification = await _db.Notifications.FirstOrDefaultAsync(m => m.Id == notificationReadRequest.Id);

            var notificationReadResponse = _mapper.Map<NotificationReadResponse>(notification);

            return new OkObjectResult(notificationReadResponse);
        }
        #endregion

        #region Update
        [FunctionName("NotificationUpdateRequest")]
        [OpenApiOperation(operationId: "NotificationUpdateRequest", tags: new[] { "Notifications" })]
        [OpenApiRequestBody(MediaTypeNames.Application.Json, typeof(NotificationUpdateRequest))]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The **Id** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json, bodyType: typeof(NotificationUpdateResponse), Description = "The OK response")]
        public async Task<IActionResult> Update(
            [HttpTrigger(AuthorizationLevel.Anonymous, HttpRequestMethods.Put, Route = "notifications/{id}")] NotificationUpdateRequest notificationUpdateRequest, int id)
        {
            var notification = await _db.Notifications.FirstOrDefaultAsync(m => m.Id == id);

            if (!string.IsNullOrWhiteSpace(notificationUpdateRequest.ModifyBy))
                notification.ModifyBy = notificationUpdateRequest.ModifyBy;
            
            if (!string.IsNullOrWhiteSpace(notificationUpdateRequest.Content))
                notification.Content = notificationUpdateRequest.Content;
                
            notification.ModifyDate = DateTimeOffset.UtcNow;

            _db.Notifications.Update(notification);

            await _db.SaveChangesAsync();

            var notificationUpdateResponse = _mapper.Map<NotificationUpdateResponse>(notification);

            return new OkObjectResult(notificationUpdateResponse);
        }
        #endregion

        #region Delete
        [FunctionName("NotificationDeleteRequest")]
        [OpenApiOperation(operationId: "NotificationDeleteRequest", tags: new[] { "Notifications" })]
        [OpenApiParameter(name: "id", In = ParameterLocation.Path, Required = true, Type = typeof(int), Description = "The **Id** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json, bodyType: typeof(NotificationDeleteResponse), Description = "The OK response")]
        public async Task<IActionResult> Delete(
            [HttpTrigger(AuthorizationLevel.Anonymous, HttpRequestMethods.Delete, Route = "notifications/{id}")] NotificationDeleteRequest notificationDeleteRequest)
        {
            var notification = await _db.Notifications.FirstOrDefaultAsync(m => m.Id == notificationDeleteRequest.Id);

            notification.StopDate = DateTimeOffset.UtcNow;

            _db.Notifications.Update(notification);

            await _db.SaveChangesAsync();

            var notificationDeleteResponse = _mapper.Map<NotificationDeleteResponse>(notification);

            return new OkObjectResult(notificationDeleteResponse);
        }
        #endregion

        #region List
        [FunctionName("NotificationListRequest")]
        [OpenApiOperation(operationId: "NotificationListRequest", tags: new[] { "Notifications" })]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = false, Type = typeof(List<int>), Description = "The **Id** parameter")]
        [OpenApiParameter(name: "skip", In = ParameterLocation.Query, Required = false, Type = typeof(int?), Description = "The **Skip** parameter")]
        [OpenApiParameter(name: "pageSize", In = ParameterLocation.Query, Required = false, Type = typeof(int?), Description = "The **PageSize** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: MediaTypeNames.Application.Json, bodyType: typeof(NotificationListResponse), Description = "The OK response")]
        public async Task<IActionResult> List(
        [HttpTrigger(AuthorizationLevel.Anonymous, HttpRequestMethods.Get, Route = "notifications")] NotificationListRequest notificationListRequest)
        {
            var query = _db.Notifications.AsQueryable();

            if (notificationListRequest.Ids.Any())
                query = query.Where(m => notificationListRequest.Ids.Any(mm => mm == m.Id));

            if (notificationListRequest.Skip.HasValue)
                query = query.Skip(notificationListRequest.Skip.Value);

            if (notificationListRequest.PageSize.HasValue)
                query = query.Take(notificationListRequest.PageSize.Value);

            var notifications = await query.ToListAsync();

            var notificationListResponseItems = notifications.Select(m => _mapper.Map<NotificationListResponseItem>(m)).ToList();

            var notificationListResponse = new NotificationListResponse
            {
                Items = notificationListResponseItems,
                Pager = new PaginatedResponse
                {
                    Total = notificationListResponseItems.Count,
                    Skip = notificationListRequest.Skip ?? 0,
                    PageSize = notificationListRequest.PageSize ?? notificationListResponseItems.Count
                }
            };

            return new OkObjectResult(notificationListResponse);
        }
        #endregion

    }
}