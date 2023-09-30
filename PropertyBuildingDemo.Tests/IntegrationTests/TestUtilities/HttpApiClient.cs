using PropertyBuildingDemo.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Constraints;
using PropertyBuildingDemo.Domain.Entities.Identity;
using PropertyBuildingDemo.Tests.Factories;

namespace PropertyBuildingDemo.Tests.IntegrationTests.TestUtilities
{
    public class HttpApiClient : IDisposable
    {
        private readonly HttpClient _client;

        public HttpApiClient(HttpClient client)
        {
            _client = client;
        }

        public Task SetTokenAuthorizationHeader(TokenResponse tokenResponse)
        {
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("access_token", tokenResponse.Token);
            return Task.CompletedTask;
        }
        public async Task<ApiResult<T>> ValidateResponseAndGetData<T>(HttpResponseMessage response, EqualConstraint equalConstraint)
        {
            
            // Assert: Verify the response
            Assert.That(response.StatusCode, equalConstraint, $"Status code must be {equalConstraint.Description}");

            // Deserialize the response content to a strongly typed object
            return await response.Content.ReadFromJsonAsync<ApiResult<T>>();
        }

        public async Task<ApiResult<T>> MakePostRequestAsync<T>(string endpoint, object requestData, EqualConstraint equalConstraint)
        {
            // Act: Make the HTTP request
            var response = await _client.PostAsJsonAsync(endpoint, requestData);

            // Validate the httpResponse and deserialize it to ApiResult<T>
            return await ValidateResponseAndGetData<T>(response, equalConstraint);
        }

        public async Task<ApiResult<T>> MakeGetRequestAsync<T>(string endpoint, EqualConstraint equalConstraint)
        {
            // Act: Make the HTTP request
            var response = await _client.GetAsync(endpoint);

            // Validate the httpResponse and deserialize it to ApiResult<T>
            return await ValidateResponseAndGetData<T>(response, equalConstraint);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
