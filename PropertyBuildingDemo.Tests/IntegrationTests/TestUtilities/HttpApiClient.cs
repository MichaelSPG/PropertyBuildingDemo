using NUnit.Framework.Constraints;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities.Identity;
using System.Net.Http.Json;

namespace PropertyBuildingDemo.Tests.IntegrationTests.TestUtilities
{
    /// <summary>
    /// Represents an HTTP client for making API requests.
    /// </summary>
    public class HttpApiClient : IDisposable
    {
        /// <summary>
        /// Enumeration of HTTP request types.
        /// </summary>
        public enum RequestType
        {
            Get, Post, Put, Delete
        }

        private readonly HttpClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpApiClient"/> class.
        /// </summary>
        /// <param name="client">The HttpClient instance to use for making requests.</param>
        public HttpApiClient(HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Sets the authorization header with an access token.
        /// </summary>
        /// <param name="tokenResponse">The token response containing the access token.</param>
        /// <returns>A completed Task.</returns>
        public Task SetTokenAuthorizationHeader(TokenResponse tokenResponse)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("access_token", tokenResponse.Token);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Makes an asynchronous API request.
        /// </summary>
        /// <typeparam name="T">The type of response data expected.</typeparam>
        /// <param name="endpoint">The API endpoint to make the request to.</param>
        /// <param name="requestType">The type of HTTP request to make (e.g., GET, POST, PUT, DELETE).</param>
        /// <param name="equalConstraint">An optional NUnit EqualConstraint to assert the response against.</param>
        /// <param name="requestData">The request data to send with the request (for POST and PUT requests).</param>
        /// <returns>An ApiResult containing the response data.</returns>
        public async Task<ApiResult<T>> MakeApiRequestAsync<T>(string endpoint, RequestType requestType = RequestType.Get, EqualConstraint equalConstraint = null, object requestData = null)
        {
            HttpResponseMessage response = null;
            ApiResult<T> result = null;

            switch (requestType)
            {
                case RequestType.Get:
                    response = await _client.GetAsync(endpoint);
                    break;
                case RequestType.Post:
                    response = await _client.PostAsJsonAsync(endpoint, requestData);
                    break;
                case RequestType.Put:
                    response = await _client.PutAsJsonAsync(endpoint, requestData);
                    break;
                case RequestType.Delete:
                    response = await _client.DeleteAsync(endpoint);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(requestType), requestType, null);
            }

            var message = await response.Content.ReadAsStringAsync();

            if (!string.IsNullOrWhiteSpace(message))
            {
                result = await response.Content.ReadFromJsonAsync<ApiResult<T>>();
            }

            if (equalConstraint != null)
            {
                // Assert: Verify the response
                Assert.That(response.StatusCode, equalConstraint, result?.GetJoinedMessages());
            }

            // Deserialize the response content to a strongly typed object
            return result;
        }

        /// <summary>
        /// Makes an asynchronous API GET request.
        /// </summary>
        /// <typeparam name="T">The type of response data expected.</typeparam>
        /// <param name="endpoint">The API endpoint to make the request to.</param>
        /// <param name="equalConstraint">An optional NUnit EqualConstraint to assert the response against.</param>
        /// <returns>An ApiResult containing the response data.</returns>
        public async Task<ApiResult<T>> MakeApiGetRequestAsync<T>(string endpoint, EqualConstraint equalConstraint = null)
        {
            return await MakeApiRequestAsync<T>(endpoint, RequestType.Get, equalConstraint);
        }

        /// <summary>
        /// Makes an asynchronous API POST request.
        /// </summary>
        /// <typeparam name="T">The type of response data expected.</typeparam>
        /// <param name="endpoint">The API endpoint to make the request to.</param>
        /// <param name="equalConstraint">An optional NUnit EqualConstraint to assert the response against.</param>
        /// <param name="requestData">The request data to send with the request.</param>
        /// <returns>An ApiResult containing the response data.</returns>
        public async Task<ApiResult<T>> MakeApiPostRequestAsync<T>(string endpoint, EqualConstraint equalConstraint = null, object requestData = null)
        {
            return await MakeApiRequestAsync<T>(endpoint, RequestType.Post, equalConstraint, requestData);
        }

        /// <summary>
        /// Makes an asynchronous API PUT request.
        /// </summary>
        /// <typeparam name="T">The type of response data expected.</typeparam>
        /// <param name="endpoint">The API endpoint to make the request to.</param>
        /// <param name="equalConstraint">An optional NUnit EqualConstraint to assert the response against.</param>
        /// <param name="requestData">The request data to send with the request.</param>
        /// <returns>An ApiResult containing the response data.</returns>
        public async Task<ApiResult<T>> MakeApiPutRequestAsync<T>(string endpoint, EqualConstraint equalConstraint = null, object requestData = null)
        {
            return await MakeApiRequestAsync<T>(endpoint, RequestType.Put, equalConstraint, requestData);
        }

        /// <summary>
        /// Makes an asynchronous API DELETE request.
        /// </summary>
        /// <typeparam name="T">The type of response data expected.</typeparam>
        /// <param name="endpoint">The API endpoint to make the request to.</param>
        /// <param name="equalConstraint">An optional NUnit EqualConstraint to assert the response against.</param>
        /// <returns>An ApiResult containing the response data.</returns>
        public async Task<ApiResult<T>> MakeApiDeleteRequestAsync<T>(string endpoint, EqualConstraint equalConstraint = null)
        {
            return await MakeApiRequestAsync<T>(endpoint, RequestType.Delete, equalConstraint);
        }

        /// <summary>
        /// Disposes of the HttpClient instance.
        /// </summary>
        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
