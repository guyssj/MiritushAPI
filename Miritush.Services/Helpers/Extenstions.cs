using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Miritush.Services.Helpers
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<T> AssertResultAsync<T>(this HttpResponseMessage response)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            options.Converters.Add(new JsonStringEnumConverter());
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(responseJson, options: options);
            }
            else
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                //var res = JsonHelper.Deserialize<GatewayException>(responseJson);
                // throw new BadGatewayException(res.ExceptionMessage);
                throw new Exception("error from SocketAPI");
            }
        }
    }

    public static class HttpClientFactoryExtensions
    {
        public static SocketAPIHttpClient GetSocketAPISenderClient(this IHttpClientFactory clientFactory)
        {
            return new SocketAPIHttpClient(clientFactory);
        }
        public static GlobalSmsHttpClient GetGlobalSmsSenderClient(this IHttpClientFactory clientFactory)
        {
            return new GlobalSmsHttpClient(clientFactory);
        }
    }
}