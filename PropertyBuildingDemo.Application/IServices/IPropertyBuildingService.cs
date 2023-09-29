using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities;

namespace PropertyBuildingDemo.Application.Services
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
        Task<Property> CreatePropertyBuilding(PropertyDto inPropertyDto);

        /// <summary>
        /// Adds an image to a property.
        /// </summary>
        /// <param name="inImageDto">The property image data.</param>
        /// <returns>The added property image.</returns>
        Task<PropertyImage> AddImageFromProperty(PropertyImageDto inImageDto);

        /// <summary>
        /// Changes the price of a property.
        /// </summary>
        /// <param name="inIdProperty">The ID of the property to update.</param>
        /// <param name="inNewPrice">The new price for the property.</param>
        /// <returns>The updated property with the new price.</returns>
        Task<Property> ChangePrice(long inIdProperty, decimal inNewPrice);

        /// <summary>
        /// Updates a property building.
        /// </summary>
        /// <param name="inPropertyDto">The updated property building data.</param>
        /// <returns>The updated property building.</returns>
        Task<Property> UpdatePropertyBuilding(PropertyDto inPropertyDto);

        /// <summary>
        /// Filters property buildings based on specified query filter arguments.
        /// </summary>
        /// <param name="inFilterArgs">The filter arguments to apply.</param>
        /// <returns>The filtered list of property buildings.</returns>
        Task<IEnumerable<Property>> FilterPropertyBuildings(DefaultQueryFilterArgs inFilterArgs);
    }
}
