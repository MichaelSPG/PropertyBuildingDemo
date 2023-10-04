using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Tests.Helpers;

namespace PropertyBuildingDemo.Tests.Factories
{
    /// <summary>
    /// Factory class for creating random valid instances of <see cref="PropertyTraceDto"/>.
    /// </summary>
    public class PropertyTraceDataFactory : IDataFactory<PropertyTraceDto>
    {
        /// <summary>
        /// Creates a valid instance of <see cref="PropertyTraceDto"/> with random data.
        /// </summary>
        /// <returns>A valid <see cref="PropertyTraceDto"/> instance.</returns>
        public PropertyTraceDto CreateValidEntityDto()
        {
            byte[] buffer = new byte[201];
            Utilities.Random.NextBytes(buffer);
            return new PropertyTraceDto()
            {
                IdProperty = 0,
                Value = (decimal)Utilities.Random.NextDouble() * 1000,
                DateSale = DateTime.Now.AddDays(-Utilities.Random.Next(40, 2000)),
                Tax = (decimal)Utilities.Random.NextDouble() * 1000,
                Name = Utilities.RandomGenerators.GenerateRandomBuildingName()
            };
        }

        /// <summary>
        /// Creates a list of valid instances of <see cref="PropertyTraceDto"/> with random data.
        /// </summary>
        /// <param name="count">The number of instances to create.</param>
        /// <param name="refId">The reference ID (optional).</param>
        /// <param name="param">An optional parameter (not used).</param>
        /// <returns>A list of valid <see cref="PropertyTraceDto"/> instances.</returns>
        public IEnumerable<PropertyTraceDto> CreateValidEntityDtoList(int count, long refId, int param)
        {
            var propertyImages = new List<PropertyTraceDto>();

            while (propertyImages.Count < count)
            {
                var propertyImage = CreateRandomPropertyTrace(refId);

                propertyImages.Add(propertyImage);
            }
            return propertyImages;
        }

        /// <summary>
        /// Creates a random <see cref="PropertyTraceDto"/> instance with the given property ID.
        /// </summary>
        /// <param name="idProperty">The property ID.</param>
        /// <returns>A random <see cref="PropertyTraceDto"/> instance.</returns>
        public static PropertyTraceDto CreateRandomPropertyTrace(long idProperty)
        {
            var owner = new PropertyTraceDto()
            {
                IdProperty = idProperty,
                Value = (decimal)Utilities.Random.NextDouble() * 1000,
                DateSale = DateTime.Now.AddDays(-Utilities.Random.Next(40, 2000)),
                Tax = (decimal)Utilities.Random.NextDouble() * 1000,
                Name = Utilities.RandomGenerators.GenerateRandomBuildingName()
            };

            return owner;
        }
    }
}
