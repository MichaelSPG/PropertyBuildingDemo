using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Tests.Factories
{
    /// <summary>
    /// Represents a generic interface for a data factory that creates valid entity DTOs.
    /// </summary>
    /// <typeparam name="TEntityDto">The type of the entity DTO to create.</typeparam>
    public interface IDataFactory<out TEntityDto>
    {
        /// <summary>
        /// Creates a valid entity DTO.
        /// </summary>
        /// <returns>A valid entity DTO instance.</returns>
        TEntityDto CreateValidEntityDto();

        /// <summary>
        /// Creates a list of valid entity DTOs.
        /// </summary>
        /// <param name="count">The number of entity DTOs to create in the list.</param>
        /// <param name="refId">An optional reference identifier for the entity DTOs.</param>
        /// <param name="param">An optional parameter for entity DTO creation.</param>
        /// <returns>A list of valid entity DTO instances.</returns>
        IEnumerable<TEntityDto> CreateValidEntityDtoList(int count, long refId = 0, int param = 0);
    }
}
