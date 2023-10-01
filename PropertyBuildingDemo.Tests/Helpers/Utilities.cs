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
        public static Random Random = new Random();
        public static void ValidateApiResultData<TData>(ApiResult<TData> result)
        {
            Assert.NotNull(result);
            Assert.NotNull(result, $"Content must be of type 'ApiResult<{nameof(TData)}>'");
            Assert.IsTrue(result.Success, $"ApiResult must be success, response is {result.GetJoinedMessages()}");
            Assert.NotNull(result.Data, $"ApiResult data must not be null");
        }

        public static void ValidateApiResult_ExpectedNotOk<TData>(ApiResult<TData> result)
        {
            Assert.NotNull(result, $"Content must be of type 'ApiResult<{nameof(TData)}>'");
            Assert.IsFalse(result.Success, $"ApiResult must be false, response is {result.GetJoinedMessages()}");
            Assert.Null(result.Data, $"ApiResult data must be null");
        }

        public static void ValidateApiResultMessage_ExpectContainsValue<TData>(ApiResult<TData> result,
            string valueExpectedToContains)
        {
            Assert.IsTrue(
                result.GetJoinedMessages().IndexOf(valueExpectedToContains, StringComparison.OrdinalIgnoreCase) >= 0,
                $"Result message must contain '{valueExpectedToContains}'. Actual message: {result.GetJoinedMessages()}"
            );
        }
        public static void ValidateApiResultMessage_ExpectContainsValue<TData>(ApiResult<TData> result,
            string[] valueExpectedToContains)
        {
            bool containsExpectedValue = valueExpectedToContains.Any(expectedValue =>
                result.GetJoinedMessages().IndexOf(expectedValue, StringComparison.OrdinalIgnoreCase) >= 0);

            Assert.IsTrue(containsExpectedValue, $"Result message must contain one of the expected values. Actual message: {result.GetJoinedMessages()}");
        }
        public static void InitializeDbForTests(PropertyBuildingContext db)
        {
            // TODO: Default values
        }

        public static void ReinitializeDbForTests(PropertyBuildingContext db)
        {
            InitializeDbForTests(db);
        }
    }
}
