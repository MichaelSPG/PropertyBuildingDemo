using PropertyBuildingDemo.Domain.Entities;

namespace PropertyBuildingDemo.Domain.Interfaces
{
    /// <summary>
    /// Represents a service interface for managing property traces.
    /// </summary>
    public interface IPropertyTraceService
    {
        /// <summary>
        /// Adds multiple property traces asynchronously.
        /// </summary>
        /// <param name="inPropertyTraces">The collection of property traces to add.</param>
        /// <returns>A task representing the asynchronous operation and the added property traces.</returns>
        Task<IEnumerable<PropertyTrace>> AddMultipleTraces(IEnumerable<PropertyTrace> inPropertyTraces);
    }
}
