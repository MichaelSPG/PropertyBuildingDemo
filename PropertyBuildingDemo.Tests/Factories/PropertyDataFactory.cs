using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Tests.Helpers;

namespace PropertyBuildingDemo.Tests.Factories
{
    public class PropertyDataFactory : IDataFactory<PropertyDto>
    {
        /// <summary>
        /// Creates a valid PropertyDto instance with random data.
        /// </summary>
        /// <returns>A valid PropertyDto instance.</returns>
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

        /// <summary>
        /// Creates a list of valid PropertyDto instances with random data.
        /// </summary>
        /// <param name="count">The number of PropertyDto instances to generate.</param>
        /// <param name="ownerId">The owner ID to assign to properties.</param>
        /// <param name="param">An unused parameter.</param>
        /// <returns>A list of valid PropertyDto instances.</returns>
        public IEnumerable<PropertyDto> CreateValidEntityDtoList(int count, long ownerId, int param)
        {
            var propertyDtos = new List<PropertyDto>();
            var usedNames = new HashSet<string>();

            while (propertyDtos.Count < count)
            {
                var displayName = Utilities.RandomGenerators.GenerateUniqueRandomName();

                // Ensure property names are unique
                if (usedNames.Contains(displayName))
                {
                    continue;
                }

                var property = new PropertyDto
                {
                    Name = displayName,
                    Year = Utilities.RandomGenerators.GenerateRandomDateInPast().Year,
                    Price = Utilities.RandomGenerators.GenerateRandomDecimal(10000, 1000000),
                    Address = Utilities.RandomGenerators.GenerateValidRandomAddress(),
                    IdOwner = ownerId
                };

                propertyDtos.Add(property);
                usedNames.Add(displayName);
            }

            usedNames.Clear();
            return propertyDtos;
        }
    }
}
