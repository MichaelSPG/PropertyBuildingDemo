using PropertyBuildingDemo.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Duende.IdentityServer.Models;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Infrastructure.Data.Identity;

namespace PropertyBuildingDemo.Tests.Helpers
{
    public class Utilities
    {
        public static void ValidateApiResultData<TData>(ApiResult<TData> result)
        {
            Assert.NotNull(result, $"Content must be of type 'ApiResult<{nameof(TData)}>'");
            Assert.IsTrue(result.Success, $"ApiResult must be success {string.Join(", ", result?.Message?.ToArray())}");
            Assert.NotNull(result.Data, $"ApiResult data must not be null");
        }

        public static void ValidateApiResult_ExpectedNotOk<TData>(ApiResult<TData> result)
        {
            Assert.NotNull(result, $"Content must be of type 'ApiResult<{nameof(TData)}>'");
            Assert.IsFalse(result.Success, $"ApiResult must be false");
            Assert.Null(result.Data, $"ApiResult data must be null");
        }
    }
}
