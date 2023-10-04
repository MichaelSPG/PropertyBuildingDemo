using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Tests.Helpers;

namespace PropertyBuildingDemo.Tests.Factories
{
    /// <summary>
    /// Factory class for creating random valid instances of <see cref="PropertyImageDto"/>.
    /// </summary>
    public class PropertyImageDataFactory : IDataFactory<PropertyImageDto>
    {
        /// <summary>
        /// Creates a valid instance of <see cref="PropertyImageDto"/> with random data.
        /// </summary>
        /// <returns>A valid <see cref="PropertyImageDto"/> instance.</returns>
        public PropertyImageDto CreateValidEntityDto()
        {
            byte[] buffer = new byte[201];
            Utilities.Random.NextBytes(buffer);
            return new PropertyImageDto()
            {
                IdProperty = 0,
                File = Utilities.RandomGenerators.GenerateRandomByteArray(),
                Enabled = true,
            };
        }

        /// <summary>
        /// Creates a list of valid instances of <see cref="PropertyImageDto"/> with random data.
        /// </summary>
        /// <param name="count">The number of instances to create.</param>
        /// <param name="refId">The reference ID (optional).</param>
        /// <param name="param">An optional parameter (not used).</param>
        /// <returns>A list of valid <see cref="PropertyImageDto"/> instances.</returns>
        public IEnumerable<PropertyImageDto> CreateValidEntityDtoList(int count, long refId, int param)
        {
            var propertyImages = new List<PropertyImageDto>();

            while (propertyImages.Count < count)
            {
                var propertyImage = param <= 0 ? CreateRandomPropertyImage(refId) : CreateRandomPropertyImage(refId, param, param);

                propertyImages.Add(propertyImage);
            }
            return propertyImages;
        }

        /// <summary>
        /// Creates a random <see cref="PropertyImageDto"/> instance with the given property ID and optional file size range.
        /// </summary>
        /// <param name="idProperty">The property ID.</param>
        /// <param name="fileMinSize">The minimum file size (optional).</param>
        /// <param name="fileMaxSize">The maximum file size (optional).</param>
        /// <returns>A random <see cref="PropertyImageDto"/> instance.</returns>
        public static PropertyImageDto CreateRandomPropertyImage(long idProperty, int fileMinSize = 201, int fileMaxSize = 1026)
        {
            var owner = new PropertyImageDto()
            {
                IdProperty = idProperty,
                File = Utilities.RandomGenerators.GenerateRandomByteArray(fileMinSize, fileMaxSize),
                Enabled = false,
            };

            return owner;
        }
    }
}
