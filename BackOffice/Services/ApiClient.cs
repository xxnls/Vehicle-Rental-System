using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackOffice.Services
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiClient()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(Properties.Settings.Default.ApiBaseUrl)
            };

            // JSON serialization options
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                WriteIndented = true
            };
        }

        /// <summary>
        /// Sends a GET request and deserializes the response to the specified type.
        /// </summary>
        public async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
        }


        public async Task<T> GetAsync<T>(string endpoint, int? id)
        {
            var response = await _httpClient.GetAsync($"{endpoint}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>(_jsonOptions);
        }

        /// <summary>
        /// Sends a POST request with data as JSON.
        /// </summary>
        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            var response = await _httpClient.PostAsJsonAsync(endpoint, data, _jsonOptions);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>(_jsonOptions);
        }

        /// <summary>
        /// Sends a PUT request with data as JSON.
        /// </summary>
        public async Task PutAsync<TRequest>(string endpoint, TRequest data)
        {
            var response = await _httpClient.PutAsJsonAsync(endpoint, data, _jsonOptions);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Sends a DELETE request.
        /// </summary>
        public async Task DeleteAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(endpoint);
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Adds an authorization header.
        /// </summary>
        public void SetAuthorizationHeader(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }
}
