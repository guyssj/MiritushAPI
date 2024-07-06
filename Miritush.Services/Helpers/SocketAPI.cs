using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Miritush.Services.Helpers
{
    public class SocketAPIHttpClient
    {
        private readonly string clientName = "SocketAPI";
        private readonly IHttpClientFactory clientFactory;

        public SocketAPIHttpClient(IHttpClientFactory clientFactory)
        {
            this.clientFactory = clientFactory;
        }

        public SocketAPIHttpRequest WithUri()
        {
            var client = this.clientFactory.CreateClient(this.clientName);

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri($"{client.BaseAddress}"),
            };

            return new SocketAPIHttpRequest(client, request);
        }
    }

    public class SocketAPIHttpRequest
    {
        private readonly HttpClient httpClient;
        private readonly HttpRequestMessage request;
        private HttpResponseMessage response;

        public SocketAPIHttpRequest(
            HttpClient httpClient,
            HttpRequestMessage message)
        {
            this.httpClient = httpClient;
            this.request = message;
        }

        public SocketAPIHttpRequest WithHeader(string name, string value)
        {
            request.Headers.Add(name, value);
            return this;
        }
        public SocketAPIHttpRequest WithMethod(string methodName)
        {

            request.RequestUri = new Uri($"{httpClient.BaseAddress}{methodName}");
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

}