using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Tests.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyBuildingDemo.Tests.Helpers;

namespace PropertyBuildingDemo.Tests.IntegrationTests
{
    /// <summary>
    /// Base test class for property-related tests.
    /// </summary>
    public class PropertyBaseTest : BaseTest
    {
        /// <summary>
        /// List of valid owner DTOs for testing purposes.
        /// </summary>
        protected List<OwnerDto> OwnerValidList;

        /// <summary>
        /// List of valid property DTOs for testing purposes.
        /// </summary>
        protected List<PropertyDto> PropertyValidList;

        /// <summary>
        /// Gets a list of valid owner IDs from a list of owner DTOs.
        /// </summary>
        /// <param name="owners">The list of owner DTOs to extract IDs from.</param>
        /// <returns>A list of valid owner IDs.</returns>
        protected List<long> GetValidOwnerIdList(List<OwnerDto> owners = null)
        {
            owners ??= OwnerValidList;
            return owners.Select(x => x.IdOwner).ToList();
        }

        /// <summary>
        /// Inserts a list of valid property DTOs into the database.
        /// </summary>
        /// <param name="count">The number of property DTOs to insert.</param>
        /// <param name="ownerId">The owner ID for the properties.</param>
        /// <param name="sameOwner">Flag indicating whether all properties have the same owner.</param>
        /// <param name="minImages">Minimum number of images for each property.</param>
        /// <param name="minTraces">Minimum number of traces for each property.</param>
        /// <returns>A list of inserted property DTOs.</returns>
        protected async Task<List<PropertyDto>> InsertValidPropertyDtoList(int count, long ownerId = 0, bool sameOwner = true, int minImages = 0, int minTraces = 0)
        {
            return await this.InsertListOfEntity<Property, PropertyDto>(
                PropertyBuildingDataFactory.GenerateRandomValidProperties(
                    count,
                    GetValidOwnerIdList(),
                    ownerId,
                    sameOwner,
                    minImages, minTraces));
        }

        /// <summary>
        /// Sets up the property-related test environment, including valid owner and property lists.
        /// </summary>
        /// <param name="insertDefaultOwnerList">Flag indicating whether to insert the default owner list into the database.</param>
        protected async Task SetupPropertyTest(bool insertDefaultOwnerList = true)
        {
            PropertyValidList = PropertyBuildingDataFactory.PropertyDataFactory.CreateValidEntityDtoList(ValidTestEntityCount).ToList();
            OwnerValidList = PropertyBuildingDataFactory.OwnerDataFactory.CreateValidEntityDtoList(ValidTestEntityCount).ToList();

            if (insertDefaultOwnerList)
            {
                OwnerValidList = await InsertListOfEntity<Owner, OwnerDto>(OwnerValidList);
            }
            else
            {
                // Insert owners as needed (e.g., directly from services or using the API).
            }
        }
    }
}
