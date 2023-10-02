using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Tests.Helpers;

namespace PropertyBuildingDemo.Tests.Factories
{
    public class OwnerDataFactory : IDataFactory<OwnerDto>
    {
        public OwnerDto CreateValidEntityDto()
        {
            var buffer = new byte[201];
            Utilities.Random.NextBytes(buffer);
            return new OwnerDto()
            {
                BirthDay = DateTime.Now.AddYears(-20),
                Address = Utilities.RandomGenerators.GenerateValidRandomAddress(),
                Name = "Alfred Newman",
                Photo = buffer,
            };
        }

        public IEnumerable<OwnerDto> CreateValidEntityDtoList(int count, long unused)
        {
            var ownerDtos = new List<OwnerDto>();

            var usedNames = new HashSet<string>();

            while (ownerDtos.Count < count)
            {
                var displayName = Utilities.RandomGenerators.GenerateUniqueRandomName();

                // Ensure email and identification number are unique
                if (usedNames.Contains(displayName))
                {
                    continue;
                }

                var owner = new OwnerDto
                {
                    Name = displayName,
                    BirthDay = Utilities.RandomGenerators.GenerateValidAgeRandomDate(),
                    Photo = Utilities.RandomGenerators.GenerateRandomByteArray(),
                    Address = Utilities.RandomGenerators.GenerateValidRandomAddress()
                };

                ownerDtos.Add(owner);
                usedNames.Add(displayName);
            }
            usedNames.Clear();
            return ownerDtos;
        }

        private static readonly HashSet<string> GeneratedNames = new HashSet<string>();
    }
}
