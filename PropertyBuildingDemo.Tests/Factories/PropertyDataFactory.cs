using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Tests.Helpers;

namespace PropertyBuildingDemo.Tests.Factories
{
    public class PropertyDataFactory : IDataFactory<PropertyDto>
    {
        public PropertyDto CreateValidEntityDto()
        {
            var buffer = new byte[201];
            Utilities.Random.NextBytes(buffer);
            return new PropertyDto()
            {
                Name = "Christoper Creator",
                Year = 2010,
                Price = 1374293.33M,
                Address = "St. Francis 223 av 11 - Miami, FL"
            };
        }

        public IEnumerable<PropertyDto> CreateValidEntityDtoList(int count, long ownerId)
        {
            var ownerDtos = new List<PropertyDto>();

            var usedNames = new HashSet<string>();

            while (ownerDtos.Count < count)
            {
                var displayName = Utilities.RandomGenerators.GenerateUniqueRandomName();

                // Ensure email and identification number are unique
                if (usedNames.Contains(displayName))
                {
                    continue;
                }

                var owner = new PropertyDto
                {
                    Name = displayName,
                    Year = Utilities.RandomGenerators.GenerateRandomDateInPast().Year,
                    Price = Utilities.RandomGenerators.GenerateRandomDecimal(10000, 1000000),
                    Address = Utilities.RandomGenerators.GenerateValidRandomAddress(),
                    IdOwner = ownerId
                };

                ownerDtos.Add(owner);
                usedNames.Add(displayName);
            }
            usedNames.Clear();
            return ownerDtos;
        }
    }
}
