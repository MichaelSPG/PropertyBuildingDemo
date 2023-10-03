using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Tests.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Tests.IntegrationTests.PerformanceFixtures
{
    public class BenchmarkingTests : BaseTest
    {
        private List<PropertyDto> _propertyValidList;
        private List<OwnerDto> _ownerValidList;
        protected const int ValidTestEntityCount = 1000;

        [SetUp]
        public async Task Setup()
        {
            await SetupValidRegistrationUser();

            _propertyValidList = PropertyBuildingDataFactory.PropertyDataFactory.CreateValidEntityDtoList(ValidTestEntityCount).ToList();
            _ownerValidList = PropertyBuildingDataFactory.OwnerDataFactory.CreateValidEntityDtoList(ValidTestEntityCount).ToList();
            _ownerValidList = await InsertListOfEntity<Owner, OwnerDto>(_ownerValidList);
        }
    }
}
