using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Miritush.Services.Helpers
{
    public class GlobalSmsHttpClient
    {
        private readonly string clientName = "GlobalSms";
        private readonly IHttpClientFactory clientFactory;

        public GlobalSmsHttpClient(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public GlobalSmsHttpRequest WithUri()
        {
            var client = this.clientFactory.CreateClient(this.clientName);

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri($"{client.BaseAddress}"),
            };

            return new GlobalSmsHttpRequest(client, request);
        }
    }

    public class GlobalSmsHttpRequest
    {
        private readonly HttpClient httpClient;
        private readonly HttpRequestMessage request;
        private HttpResponseMessage response;

        public GlobalSmsHttpRequest(
            HttpClient httpClient,
            HttpRequestMessage message)
        {
            this.httpClient = httpClient;
            this.request = message;
        }

        public GlobalSmsHttpRequest WithHeader(string name, string value)
        {
            request.Headers.Add(name, value);
            return this;
        }
        public GlobalSmsHttpRequest Message(string message)
        {

            var uriWithQuery = QueryHelpersCustom.AddQueryString(
                request.RequestUri.OriginalString,
                "txtSMSmessage",
                message);

            request.RequestUri = new Uri(uriWithQuery);
            return this;
        }

        public GlobalSmsHttpRequest WithSender(string senderName)
        {
            var uriWithQuery = QueryHelpersCustom.AddQueryString(
                request.RequestUri.OriginalString,
                "txtOriginator",
                senderName);

            request.RequestUri = new Uri(uriWithQuery);
            return this;
        }
        public GlobalSmsHttpRequest ToPhoneNumber(string phoneNumber)
        {
            var uriWithQuery = QueryHelpersCustom.AddQueryString(
                request.RequestUri.OriginalString,
                "destinations",
                phoneNumber);

            request.RequestUri = new Uri(uriWithQuery);
            return this;
        }


        public async Task<HttpResponseMessage> PostAsync(object data)
        {
            request.Method = HttpMethod.Post;
            request.Content = new StringContent(JsonSerializer.Serialize(data),
                System.Text.Encoding.UTF8, "application/json");

            using var client = this.httpClient;
            this.response = await client.SendAsync(request);

            return this.response;
        }

        public async Task<HttpResponseMessage> GetAsync()
        {
            request.Method = HttpMethod.Get;

            using var client = this.httpClient;
            this.response = await client.SendAsync(request);

            return this.response;
        }

        public async Task<HttpResponseMessage> DeleteAsync()
        {
            request.Method = HttpMethod.Delete;

            using var client = this.httpClient;
            this.response = await client.SendAsync(request);

            return this.response;
        }
    }

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
                throw new Exception("error from GLOBALSMS");
            }
        }
    }

    public static class HttpClientFactoryExtensions
    {
        public static GlobalSmsHttpClient GetGlobalSmsSenderClient(this IHttpClientFactory clientFactory)
        {
            return new GlobalSmsHttpClient(clientFactory);
        }
    }
}