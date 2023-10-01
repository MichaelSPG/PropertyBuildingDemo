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
using Microsoft.VisualStudio.TestPlatform.Common;

namespace PropertyBuildingDemo.Tests.IntegrationTests.TestUtilities
{
    public class HttpApiClient : IDisposable
    { public enum RequestType
        {
            Get, Post, Put, Delete
        }

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
        public async Task<ApiResult<T>> MakeApiRequestAsync<T>(string endpoint, RequestType requestType = RequestType.Get, EqualConstraint equalConstraint = null, object requestData = null)
        {
            HttpResponseMessage response = null;
            ApiResult <T> result = null;
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

        public async Task<ApiResult<T>> MakeApiGetRequestAsync<T>(string endpoint, EqualConstraint equalConstraint = null)
        {
            return await MakeApiRequestAsync<T>(endpoint, RequestType.Get, equalConstraint);
        }
        public async Task<ApiResult<T>> MakeApiPostRequestAsync<T>(string endpoint, EqualConstraint equalConstraint = null, object requestData = null)
        {
            return await MakeApiRequestAsync<T>(endpoint, RequestType.Post, equalConstraint, requestData);
        }
        public async Task<ApiResult<T>> MakeApiPutRequestAsync<T>(string endpoint, EqualConstraint equalConstraint = null, object requestData = null)
        {
            return await MakeApiRequestAsync<T>(endpoint, RequestType.Put, equalConstraint, requestData);
        }
        public async Task<ApiResult<T>> MakeApiDeleteRequestAsync<T>(string endpoint, EqualConstraint equalConstraint = null)
        {
            return await MakeApiRequestAsync<T>(endpoint, RequestType.Delete, equalConstraint);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
