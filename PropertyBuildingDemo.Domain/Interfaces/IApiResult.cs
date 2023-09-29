namespace PropertyBuildingDemo.Domain.Interfaces
{
    /// <summary>
    /// Represents the result of an API operation with a generic data type.
    /// </summary>
    /// <typeparam name="TData">The type of the data included in the result.</typeparam>
    public interface IApiResult<TData>
    {
        /// <summary>
        /// Gets the data included in the result.
        /// </summary>
        TData Data { get; }
    }

    /// <summary>
    /// Represents the result of an API operation with a collection of messages, a success flag, and a result code.
    /// </summary>
    public interface IApiResult
    {
        /// <summary>
        /// Gets or sets a collection of messages associated with the result.
        /// </summary>
        IEnumerable<string> Message { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating the success of the API operation.
        /// </summary>
        bool Success { get; set; }

        /// <summary>
        /// Gets or sets a result code for the API operation.
        /// </summary>
        int ResultCode { get; set; }
    }
}
