using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Formatting = System.Xml.Formatting;

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
            if (string.IsNullOrWhiteSpace(endpoint))
            {
                throw new ArgumentException("Endpoint cannot be null or empty.", nameof(endpoint));
            }

            if (data == null)
            {
                throw new ArgumentNullException(nameof(data), "Request data cannot be null.");
            }

            try
            {
                // Serialize the request data
                var jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });

                // Log the request details
                Console.WriteLine($"Sending PUT request to: {endpoint}");
                Console.WriteLine($"Request Data: {jsonData}");

                // Ensure the HttpClient is properly configured
                if (_httpClient == null)
                {
                    throw new InvalidOperationException("HttpClient is not initialized.");
                }

                // Create the HTTP content
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Send the request
                var response = await _httpClient.PutAsync(endpoint, content);

                // Log the response status code
                Console.WriteLine($"Response Status Code: {response.StatusCode}");

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                // Optionally, log the response content
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Content: {responseContent}");
            }
            catch (HttpRequestException ex)
            {
                // Log the HTTP request exception
                Console.WriteLine($"HTTP Request Failed: {ex.Message}");

                // Capture the response content for more details
                if (ex.Data.Contains("ResponseContent"))
                {
                    Console.WriteLine($"Response Content: {ex.Data["ResponseContent"]}");
                }

                throw new ApplicationException("An error occurred while sending the request. Please check the request data and try again.", ex);
            }
            catch (TaskCanceledException ex)
            {
                // Handle timeout issues
                Console.WriteLine($"Request Timeout: {ex.Message}");
                throw new ApplicationException("The request timed out. Please check your network connection and try again.", ex);
            }
            catch (Exception ex)
            {
                // Log any other exceptions
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                throw new ApplicationException("An unexpected error occurred. Please try again later.", ex);
            }
        }
        //public async Task PutAsync<TRequest>(string endpoint, TRequest data)
        //{
        //    var response = await _httpClient.PutAsJsonAsync(endpoint, data, _jsonOptions);
        //    response.EnsureSuccessStatusCode();
        //}

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
