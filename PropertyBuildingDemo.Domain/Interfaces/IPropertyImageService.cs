using PropertyBuildingDemo.Domain.Entities;

namespace PropertyBuildingDemo.Domain.Interfaces
{
    /// <summary>
    /// Represents a service interface for managing property images.
    /// </summary>
    public interface IPropertyImageService
    {
        /// <summary>
        /// Adds multiple property images asynchronously.
        /// </summary>
        /// <param name="inPropertyImages">The collection of property images to add.</param>
        /// <returns>A task representing the asynchronous operation and the added property images.</returns>
        Task<IEnumerable<PropertyImage>> AddMultipleImages(IEnumerable<PropertyImage> inPropertyImages);
    }
}
