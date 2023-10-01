using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Common;

namespace PropertyBuildingDemo.Application.IServices
{
    /// <summary>
    /// Service for managing property building operations.
    /// </summary>
    public interface IPropertyBuildingService
    {
        /// <summary>
        /// Creates a new property building.
        /// </summary>
        /// <param name="inPropertyDto">The property building data.</param>
        /// <returns>The created property building.</returns>
        Task<PropertyDto> CreatePropertyBuilding(PropertyDto inPropertyDto);

        /// <summary>
        /// Adds an image to a property.
        /// </summary>
        /// <param name="inImageDto">The property image data.</param>
        /// <returns>The added property image.</returns>
        Task<PropertyDto> AddImageFromProperty(PropertyImageDto inImageDto);

        /// <summary>
        /// Changes the price of a property.
        /// </summary>
        /// <param name="inIdProperty">The ID of the property to update.</param>
        /// <param name="inNewPrice">The new price for the property.</param>
        /// <returns>The updated property with the new price.</returns>
        Task<PropertyDto> ChangePrice(long inIdProperty, decimal inNewPrice);

        /// <summary>
        /// Updates a property building.
        /// </summary>
        /// <param name="inPropertyDto">The updated property building data.</param>
        /// <returns>The updated property building.</returns>
        Task<PropertyDto> UpdatePropertyBuilding(PropertyDto inPropertyDto);

        /// <summary>
        /// Filters property buildings based on specified query filter arguments.
        /// </summary>
        /// <param name="inFilterArgs">The filter arguments to apply.</param>
        /// <returns>The filtered list of property buildings.</returns>
        Task<IEnumerable<PropertyDto>> FilterPropertyBuildings(DefaultQueryFilterArgs inFilterArgs);

        /// <summary>
        /// Retrieves detailed information about a property or building based on the provided identifier.
        /// </summary>
        /// <param name="inId">The unique identifier of the property or building to retrieve.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// The task result is a <see cref="PropertyDto"/> containing detailed information about the retrieved property or building.
        /// </returns>
        Task<PropertyDto> GetPropertyBuildingById(long inId);
    }
}
