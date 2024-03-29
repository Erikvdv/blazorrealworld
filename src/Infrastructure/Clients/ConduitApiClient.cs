﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Application.Exceptions;
using Application.Models;
using Application.Services;
using Infrastructure.Clients.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Clients
{
    public class ConduitApiClient : IConduitApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ConduitClientSettings _settings;
        private readonly ILogger<ConduitApiClient> _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ConduitApiClient(
            HttpClient httpClient,
            IOptions<ConduitClientSettings> settings,
            ILogger<ConduitApiClient> logger)
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

        public Task<ArticleList> GetArticleListAsync(ArticleListFilter articleListFilter, string? token, CancellationToken cancellationToken = default)
        {
            var querystring = GetQueryString(articleListFilter);
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, new Uri($"api/articles?{querystring}", UriKind.Relative));
            httpRequest.Headers.Add("Accept", "application/json");
            if (token != null)
            {
                httpRequest.Headers.Add("Authorization", $"Token {token}");
            }

            return HandleRequest<ArticleList>(httpRequest, cancellationToken);
        }

        public Task<ArticleList> GetArticleFeedAsync(ArticleListFilter articleListFilter, string token, CancellationToken cancellationToken = default)
        {
            var querystring = GetQueryString(articleListFilter);
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, new Uri($"api/articles/feed?{querystring}", UriKind.Relative));
            httpRequest.Headers
                .Add("Authorization", $"Token {token}");


            return HandleRequest<ArticleList>(httpRequest, cancellationToken);
        }

        public async Task<Article> GetArticleAsync(string slug, string? token, CancellationToken cancellationToken = default)
        {

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, new Uri($"api/articles/{slug}", UriKind.Relative));
            httpRequest.Headers.Add("Accept", "application/json");
            if (token != null)
            {
                httpRequest.Headers.Add("Authorization", $"Token {token}");
            }

            var response = await HandleRequest<ArticleObject>(httpRequest, cancellationToken);
            return response.Article;
        }

        public async Task<Article> CreateArticleAsync(NewArticle article, string token, CancellationToken cancellationToken = default)
        {
            var body = new NewArticleRequest { Article = article };
            var requestBody = JsonSerializer.Serialize(body, _jsonSerializerOptions);
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"api/articles", UriKind.Relative))
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };
            httpRequest.Headers.Add("Authorization", $"Token {token}");

            var response = await HandleRequest<ArticleObject>(httpRequest, cancellationToken);
            return response.Article;
        }

        public async Task<Article> UpdateArticleAsync(Article article, string token, CancellationToken cancellationToken = default)
        {
            var body = new ArticleObject { Article = article };
            var requestBody = JsonSerializer.Serialize(body, _jsonSerializerOptions);
            var httpRequest = new HttpRequestMessage(HttpMethod.Put, new Uri($"api/articles/{article.Slug}", UriKind.Relative))
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };
            httpRequest.Headers.Add("Authorization", $"Token {token}");

            var response = await HandleRequest<ArticleObject>(httpRequest, cancellationToken);
            return response.Article;
        }

        public async Task<List<Comment>> GetArticleCommentsAsync(string slug, string? token, CancellationToken cancellationToken = default)
        {

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, new Uri($"api/articles/{slug}/comments", UriKind.Relative));
            httpRequest.Headers.Add("Accept", "application/json");
            if (token != null)
            {
                httpRequest.Headers.Add("Authorization", $"Token {token}");
            }

            var response = await HandleRequest<CommentsResponse>(httpRequest, cancellationToken);
            return response.Comments;
        }

        public async Task<string[]> GetTagListAsync(CancellationToken cancellationToken = default)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, new Uri($"api/tags", UriKind.Relative));
            httpRequest.Headers.Add("Accept", "application/json");

            var response = await HandleRequest<GetTagsResponse>(httpRequest, cancellationToken);
            return response.Tags;
        }

        public async Task<Profile> GetProfileAsync(string username, string? token, CancellationToken cancellationToken = default)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, new Uri($"api/profiles/{username}", UriKind.Relative));
            httpRequest.Headers.Add("Accept", "application/json");
            if (token != null)
            {
                httpRequest.Headers.Add("Authorization", $"Token {token}");
            }

            var response = await HandleRequest<ProfileResponse>(httpRequest, cancellationToken);
            return response.Profile;
        }

        public async Task<User> GetUserAsync(string token, CancellationToken cancellationToken = default)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, new Uri($"api/user", UriKind.Relative));
            httpRequest.Headers.Add("Accept", "application/json");
            httpRequest.Headers.Add("Authorization", $"Token {token}");


            var response = await HandleRequest<UserResponse>(httpRequest, cancellationToken);
            return response.User;
        }

        public async Task<User> UpdateUserAsync(User user, string token, CancellationToken cancellationToken = default)
        {

            var updateUserRequest = new UserUpdateRequest { User = user };
            var requestBody = JsonSerializer.Serialize(updateUserRequest, _jsonSerializerOptions);
            var httpRequest = new HttpRequestMessage(HttpMethod.Put, new Uri($"api/user", UriKind.Relative))
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json")
            };
            httpRequest.Headers.Add("Accept", "application/json");
            httpRequest.Headers.Add("Authorization", $"Token {token}");


            var response = await HandleRequest<UserResponse>(httpRequest, cancellationToken);
            return response.User;
        }

        public async Task<Profile> FollowProfileAsync(string username, string? token, CancellationToken cancellationToken = default)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"api/profiles/{username}/follow", UriKind.Relative));
            httpRequest.Headers.Add("Accept", "application/json");
            if (token != null)
            {
                httpRequest.Headers.Add("Authorization", $"Token {token}");
            }

            var response = await HandleRequest<ProfileResponse>(httpRequest, cancellationToken);
            return response.Profile;
        }

        public async Task<Profile> UnFollowProfileAsync(string username, string? token, CancellationToken cancellationToken = default)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Delete, new Uri($"api/profiles/{username}/follow", UriKind.Relative));
            httpRequest.Headers.Add("Accept", "application/json");
            if (token != null)
            {
                httpRequest.Headers.Add("Authorization", $"Token {token}");
            }

            var response = await HandleRequest<ProfileResponse>(httpRequest, cancellationToken);
            return response.Profile;
        }

        public async Task<User> LoginAsync(Login login, CancellationToken cancellationToken = default)
        {
            var loginRequest = new LoginRequest { User = login };
            var requestBody = JsonSerializer.Serialize(loginRequest, _jsonSerializerOptions);
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"api/users/login", UriKind.Relative))
            {
                Content = new StringContent(requestBody, Encoding.UTF8, "application/json"),
            };
            var response = await HandleRequest<LoginResponse>(httpRequest, cancellationToken);
            return response.User;
        }

        public async Task<User> LoginWithTokenAsync(string token, CancellationToken cancellationToken = default)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, new Uri($"api/user", UriKind.Relative)) { };
            httpRequest.Headers.Add("authorization", $"Token {token}");
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

        public string GetQueryString(object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return String.Join("&", properties.ToArray());
        }

    }
}
