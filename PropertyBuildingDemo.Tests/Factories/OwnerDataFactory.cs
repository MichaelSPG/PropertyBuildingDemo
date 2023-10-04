using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Tests.Helpers;
using System.Linq;

namespace PropertyBuildingDemo.Tests.Factories
{
    /// <summary>
    /// Factory for creating OwnerDto objects
    /// </summary>
    public class OwnerDataFactory : IDataFactory<OwnerDto>
    {
        /// <summary>
        /// Creates a valid OwnerDto with sample data.
        /// </summary>
        public OwnerDto CreateValidEntityDto()
        {
            var buffer = Utilities.RandomGenerators.GenerateRandomByteArray(201, 1026);
            return new OwnerDto()
            {
                BirthDay = DateTime.Now.AddYears(-20),
                Address = Utilities.RandomGenerators.GenerateValidRandomAddress(),
                Name = "Alfred Newman",
                Photo = buffer,
            };
        }

        /// <summary>
        /// Creates a list of valid OwnerDto instances with unique names.
        /// </summary>
        /// <param name="count">The number of OwnerDto instances to create.</param>
        /// <param name="unused">Unused parameter (not used in this method).</param>
        /// <param name="param">Unused parameter (not used in this method).</param>
        /// <returns>A list of OwnerDto instances.</returns>
        public IEnumerable<OwnerDto> CreateValidEntityDtoList(int count, long unused, int param)
        {
            var ownerDtos = new List<OwnerDto>();

            var usedNames = new HashSet<string>();

            while (ownerDtos.Count < count)
            {
                var owner = param <= 0 ? CreateRandomOwner() : CreateRandomOwner(param, param);

                // Ensure name is unique
                if (usedNames.Contains(owner.Name))
                {
                    continue;
                }

                ownerDtos.Add(owner);
                usedNames.Add(owner.Name);
            }

            usedNames.Clear();
            return ownerDtos;
        }

        /// <summary>
        /// Creates a random OwnerDto with optional file size constraints.
        /// </summary>
        /// <param name="fileMinSize">Minimum size for the Photo property.</param>
        /// <param name="fileMaxSize">Maximum size for the Photo property.</param>
        /// <returns>A random OwnerDto instance.</returns>
        public static OwnerDto CreateRandomOwner(int fileMinSize = 201, int fileMaxSize = 1026)
        {
            var displayName = Utilities.RandomGenerators.GenerateUniqueRandomName();
            var owner = new OwnerDto
            {
                Name = displayName,
                BirthDay = Utilities.RandomGenerators.GenerateValidAgeRandomDate(),
                Photo = Utilities.RandomGenerators.GenerateRandomByteArray(fileMinSize, fileMaxSize),
                Address = Utilities.RandomGenerators.GenerateValidRandomAddress()
            };
            return owner;
        }
    }
}
