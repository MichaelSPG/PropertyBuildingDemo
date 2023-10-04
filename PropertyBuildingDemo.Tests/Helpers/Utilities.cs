using PropertyBuildingDemo.Domain.Common;

namespace PropertyBuildingDemo.Tests.Helpers
{
    public partial class Utilities
    {
        public static Random Random = new Random();

        /// <summary>
        /// Validates the API result for expected success.
        /// </summary>
        public static void ValidateApiResult_ExpectedSuccess<TData>(ApiResult<TData> result)
        {
            Assert.NotNull(result);
            Assert.NotNull(result, $"Content must be of type 'ApiResult<{nameof(TData)}>'");
            Assert.IsTrue(result.Success, $"ApiResult must be successful, response is {result.GetJoinedMessages()}");
            Assert.NotNull(result.Data, $"ApiResult data must not be null");
        }

        /// <summary>
        /// Validates the API result for expected success with null data.
        /// </summary>
        public static void ValidateApiResult_ExpectedSuccessButNullData<TData>(ApiResult<TData> result)
        {
            Assert.NotNull(result, $"Content must be of type 'ApiResult<{nameof(TData)}>'");
            Assert.IsTrue(result.Success, $"ApiResult must be successful, response is {result.GetJoinedMessages()}");
            Assert.Null(result.Data, $"ApiResult data must be null");
        }

        /// <summary>
        /// Validates the API result for expected failure.
        /// </summary>
        public static void ValidateApiResult_ExpectedFailed<TData>(ApiResult<TData> result)
        {
            Assert.NotNull(result, $"Content must be of type 'ApiResult<{nameof(TData)}>'");
            Assert.IsFalse(result.Success, $"ApiResult must be false, response is {result.GetJoinedMessages()}");
            Assert.Null(result.Data, $"ApiResult data must be null");
        }

        /// <summary>
        /// Validates that the API result message contains the expected value.
        /// </summary>
        public static void ValidateApiResultMessage_ExpectContainsValue<TData>(ApiResult<TData> result,
            string valueExpectedToContain)
        {
            Assert.IsTrue(
                result.GetJoinedMessages().IndexOf(valueExpectedToContain, StringComparison.OrdinalIgnoreCase) >= 0,
                $"Result message must contain '{valueExpectedToContain}'. Actual message: {result.GetJoinedMessages()}"
            );
        }

        /// <summary>
        /// Validates that the API result message contains one of the expected values.
        /// </summary>
        public static void ValidateApiResultMessage_ExpectContainsValue<TData>(ApiResult<TData> result,
            string[] valuesExpectedToContain)
        {
            bool containsExpectedValue = valuesExpectedToContain.Any(expectedValue =>
                result.GetJoinedMessages().IndexOf(expectedValue, StringComparison.OrdinalIgnoreCase) >= 0);

            Assert.IsTrue(containsExpectedValue, $"Result message must contain one of the expected values. Actual message: {result.GetJoinedMessages()}");
        }
    }
}