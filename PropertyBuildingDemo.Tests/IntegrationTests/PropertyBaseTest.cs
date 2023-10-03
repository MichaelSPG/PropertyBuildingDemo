using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Tests.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Tests.IntegrationTests
{
    public class PropertyBaseTest : BaseTest
    {
        protected List<OwnerDto> OwnerValidList;
        protected List<PropertyDto> PropertyValidList;

        protected List<long> GetValidOwnerIdList(List<OwnerDto> owners = null)
        {
            owners ??= OwnerValidList;
            return owners.Select(x => x.IdOwner).ToList();
        }

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

        protected async Task SetupPropertyTest()
        {
            PropertyValidList = PropertyBuildingDataFactory.PropertyDataFactory.CreateValidEntityDtoList(ValidTestEntityCount).ToList();
            OwnerValidList = PropertyBuildingDataFactory.OwnerDataFactory.CreateValidEntityDtoList(ValidTestEntityCount).ToList();
            OwnerValidList = await InsertListOfEntity<Owner, OwnerDto>(OwnerValidList);
        }
    }
}
