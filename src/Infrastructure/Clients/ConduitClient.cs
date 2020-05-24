using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Application.Clients;
using Application.Exceptions;
using Application.Models;
using Infrastructure.Clients.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Clients
{
    public class ConduitClient : IConduitClient
    {
        private readonly HttpClient _httpClient;
        private readonly ConduitClientSettings _settings;
        private readonly ILogger<ConduitClient> _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ConduitClient(
            HttpClient httpClient,
            IOptions<ConduitClientSettings> settings,
            ILogger<ConduitClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase

            };
            _httpClient.BaseAddress = new Uri(_settings.BaseAddress);
        }

        public Task<ArticleList> GetArticleListAsync(CancellationToken cancellationToken = default)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, new Uri($"api/articles?limit=10&offset=0", UriKind.Relative));
            httpRequest.Headers.Add("Accept", "application/json");

            return HandleRequest<ArticleList>(httpRequest, cancellationToken);
        }

        public async Task<string[]> GetTagListAsync(CancellationToken cancellationToken = default)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, new Uri($"api/tags", UriKind.Relative));
            httpRequest.Headers.Add("Accept", "application/json");

            var response = await HandleRequest<GetTagsResponse>(httpRequest, cancellationToken);
            return response.Tags;
        }

        public async Task<User> LoginAsync(Login login, CancellationToken cancellationToken = default)
        {
            var loginRequest = new LoginRequest { User = login };
            var requestBody = System.Text.Json.JsonSerializer.Serialize(loginRequest, _jsonSerializerOptions);
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"api/users/login", UriKind.Relative))
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json"),
            };
            var response = await HandleRequest<LoginResponse>(httpRequest, cancellationToken);
            return response.User;
        }

        private async Task<T> HandleRequest<T>(HttpRequestMessage request,
            CancellationToken cancellationToken = default)
        {

            var response = await _httpClient.SendAsync(request, cancellationToken);
            var responseBody = await GetResponseBody(response);
            _logger.LogTrace($"Request: {request.RequestUri}, HTTP {(int)response.StatusCode}: {responseBody}.");

            if (response.IsSuccessStatusCode)
            {
                return System.Text.Json.JsonSerializer.Deserialize<T>(responseBody, _jsonSerializerOptions);
            }

            _logger.LogError($"Error executing request to '{request.RequestUri}', HTTP {(int)response.StatusCode}: {responseBody}");
            if (response.StatusCode == HttpStatusCode.UnprocessableEntity)
            {
                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseBody, _jsonSerializerOptions);


                throw new ApiException(errorResponse.Errors);
            }

            throw new Exception($"Error executing request to '{request.RequestUri}', HTTP {(int)response.StatusCode}: {responseBody}"
            );
        }

        private static async Task<string> GetResponseBody(HttpResponseMessage response)
        {
            return response.Content != null ? await response.Content.ReadAsStringAsync() : string.Empty;
        }


    }
}
