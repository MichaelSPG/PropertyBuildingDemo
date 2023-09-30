using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PropertyBuildingDemo.Domain.Common
{
    /// <summary>
    /// Represents the result of an API operation with no specific data type.
    /// </summary>
    public class ApiResult : IApiResult
    {
        /// <summary>
        /// Gets or sets a collection of messages associated with the result.
        /// </summary>
        public IEnumerable<string> Message { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating the success of the API operation.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets a result code for the API operation.
        /// </summary>
        public int ResultCode { get; set; } = -1;

        /// <summary>
        /// Checks if the API operation was successful.
        /// </summary>
        /// <returns>True if the operation was successful; otherwise, false.</returns>
        public bool IsSuccess()
        {
            return Success;
        }

        /// <summary>
        /// Checks if the API operation failed.
        /// </summary>
        /// <returns>True if the operation failed; otherwise, false.</returns>
        public bool Failed()
        {
            return !Success;
        }

        /// <summary>
        /// Creates a successful API result with a message.
        /// </summary>
        /// <param name="inMessage">The success message.</param>
        /// <returns>A successful API result with the specified message.</returns>
        public static ApiResult SuccessResult(string inMessage)
        {
            return new ApiResult { Success = true, Message = new List<string> { inMessage } };
        }

        /// <summary>
        /// Creates a successful API result without a message.
        /// </summary>
        /// <returns>A successful API result.</returns>
        public static ApiResult SuccessResult()
        {
            return new ApiResult { Success = true };
        }

        /// <summary>
        /// Creates a failed API result with a code and a message.
        /// </summary>
        /// <param name="inCode">The error code.</param>
        /// <param name="inMessage">The error message.</param>
        /// <returns>A failed API result with the specified code and message.</returns>
        public static ApiResult FailedResult(int inCode, string inMessage)
        {
            return new ApiResult { Success = false, Message = new List<string> { inMessage }, ResultCode = inCode };
        }

        /// <summary>
        /// Creates a failed API result with a message.
        /// </summary>
        /// <param name="inMessage">The error message.</param>
        /// <returns>A failed API result with the specified message.</returns>
        public static ApiResult FailedResult(string inMessage)
        {
            return new ApiResult { Success = false, Message = new List<string> { inMessage } };
        }
        /// <summary>
        /// Creates a failed API result with a message list.
        /// </summary>
        /// <param name="inMessage">The error message list.</param>
        /// <returns>A failed API result with the specified message.</returns>
        public static ApiResult FailedResult(List<string> inMessage)
        {
            return new ApiResult { Success = false, Message =inMessage };
        }

        /// <summary>
        /// Creates a failed API result without a message.
        /// </summary>
        /// <returns>A failed API result.</returns>
        public static ApiResult FailedResult()
        {
            return new ApiResult { Success = false };
        }
    }

    /// <summary>
    /// Represents the result of an API operation with a specific data type.
    /// </summary>
    /// <typeparam name="TData">The type of data included in the result.</typeparam>
    public class ApiResult<TData> : ApiResult, IApiResult<TData>
    {
        /// <summary>
        /// Gets or sets the data included in the result.
        /// </summary>
        public TData Data { get; set; }

        //// <summary>
        /// Creates a successful API result with data and an optional message.
        /// </summary>
        /// <param name="data">The data to include in the result.</param>
        /// <param name="inMessage">The optional success message.</param>
        /// <returns>A successful API result with the specified data and message.</returns>
        public static ApiResult<TData> SuccessResult(TData data, string inMessage = null)
        {
            if (string.IsNullOrWhiteSpace(inMessage))
                return new ApiResult<TData> { Data = data, Success = true };

            return new ApiResult<TData> { Success = true, Data = data, Message = new List<string> { inMessage } };
        }

        /// <summary>
        /// Creates a successful API result with a message.
        /// </summary>
        /// <param name="inMessage">The success message.</param>
        /// <returns>A successful API result with the specified message.</returns>
        public new static ApiResult<TData> SuccessResult(string inMessage)
        {
            return new ApiResult<TData> { Success = true, Message = new List<string> { inMessage } };
        }

        /// <summary>
        /// Creates a successful API result without a message.
        /// </summary>
        /// <returns>A successful API result.</returns>
        public new static ApiResult<TData> SuccessResult()
        {
            return new ApiResult<TData> { Success = true };
        }

        /// <summary>
        /// Creates a failed API result with a code and a message.
        /// </summary>
        /// <param name="inCode">The error code.</param>
        /// <param name="inMessage">The error message.</param>
        /// <returns>A failed API result with the specified code and message.</returns>
        public new static ApiResult<TData> FailedResult(int inCode, string inMessage)
        {
            return new ApiResult<TData> { Success = false, Message = new List<string> { inMessage }, ResultCode = inCode };
        }

        /// <summary>
        /// Creates a failed API result with a message.
        /// </summary>
        /// <param name="inMessage">The error message.</param>
        /// <returns>A failed API result with the specified message.</returns>
        public new static ApiResult<TData> FailedResult(string inMessage)
        {
            return new ApiResult<TData> { Success = false, Message = new List<string> { inMessage } };
        }

        /// <summary>
        /// Creates a failed API result without a message.
        /// </summary>
        /// <returns>A failed API result.</returns>
        public new static ApiResult<TData> FailedResult()
        {
            return new ApiResult<TData> { Success = false };
        }

        /// <summary>
        /// Creates a successful API result with data and an optional message asynchronously.
        /// </summary>
        /// <param name="data">The data to include in the result.</param>
        /// <param name="inMessage">The optional success message.</param>
        /// <returns>A task representing the asynchronous operation that returns a successful API result with the specified data and message.</returns>
        public static Task<ApiResult<TData>> SuccessResultAsync(TData data, string inMessage = null)
        {
            if (string.IsNullOrWhiteSpace(inMessage))
                return Task.FromResult(new ApiResult<TData> { Data = data, Success = true });

            return Task.FromResult(new ApiResult<TData> { Success = true, Data = data, Message = new List<string> { inMessage } });
        }

        /// <summary>
        /// Creates a successful API result with a message asynchronously.
        /// </summary>
        /// <param name="inMessage">The success message.</param>
        /// <returns>A task representing the asynchronous operation that returns a successful API result with the specified message.</returns>
        public static Task<ApiResult<TData>> SuccessResultAsync(string inMessage)
        {
            return Task.FromResult(new ApiResult<TData> { Success = true, Message = new List<string> { inMessage } });
        }

        /// <summary>
        /// Creates a successful API result without a message asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation that returns a successful API result.</returns>
        public static Task<ApiResult<TData>> SuccessResultAsync()
        {
            return Task.FromResult(new ApiResult<TData> { Success = true });
        }

        /// <summary>
        /// Creates a failed API result with a code and a message asynchronously.
        /// </summary>
        /// <param name="inCode">The error code.</param>
        /// <param name="inMessage">The error message.</param>
        /// <returns>A task representing the asynchronous operation that returns a failed API result with the specified code and message.</returns>
        public static Task<ApiResult<TData>> FailedResultAsync(int inCode, string inMessage)
        {
            return Task.FromResult(new ApiResult<TData> { Success = false, Message = new List<string> { inMessage }, ResultCode = inCode });
        }

        /// <summary>
        /// Creates a failed API result with a message asynchronously.
        /// </summary>
        /// <param name="inMessage">The error message.</param>
        /// <returns>A task representing the asynchronous operation that returns a failed API result with the specified message.</returns>
        public static Task<ApiResult<TData>> FailedResultAsync(string inMessage)
        {
            return Task.FromResult(new ApiResult<TData> { Success = false, Message = new List<string> { inMessage } });
        }

        /// <summary>
        /// Creates a failed API result with multiple messages asynchronously.
        /// </summary>
        /// <param name="inMessages">The list of error messages.</param>
        /// <returns>A task representing the asynchronous operation that returns a failed API result with the specified messages.</returns>
        public static Task<ApiResult<TData>> FailedResultAsync(List<string> inMessages)
        {
            return Task.FromResult(new ApiResult<TData> { Success = false, Message = inMessages });
        }

        /// <summary>
        /// Creates a failed API result without a message asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation that returns a failed API result.</returns>
        public static Task<ApiResult<TData>> FailedResultAsync()
        {
            return Task.FromResult(new ApiResult<TData> { Success = false });
        }
        /// <summary>
        ///  Gets the messages and join them in one string and return the result
        /// </summary>
        /// <param name="separator">the separator of the joined string</param>
        /// <returns>The joined message as a single string</returns>
        public string GetJoinedMessages(string separator = ", ")
        {
            string description = "";

            if (Message == null || !Message.Any())
                return description;

            return string.Join(separator, Message);
        }
    }
}
