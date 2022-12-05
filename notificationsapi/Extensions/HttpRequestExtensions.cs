using System.IO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace NotificationsApi.Extensions
{
    public static class HttpRequestExtensions
    {
        public static T DeserializeRequest<T>(this HttpRequest request)
        {
            using var streamReader = new StreamReader(request.Body);
            using var textReader = new JsonTextReader(streamReader);
            request.Body.Seek(0, SeekOrigin.Begin);
            var serializer = JsonSerializer.Create(new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            return serializer.Deserialize<T>(textReader);
        }
    }
}