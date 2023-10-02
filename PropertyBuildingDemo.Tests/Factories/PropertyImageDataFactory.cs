using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Tests.Helpers;

namespace PropertyBuildingDemo.Tests.Factories
{
    public class PropertyImageDataFactory : IDataFactory<PropertyImageDto>
    {
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

        public IEnumerable<PropertyImageDto> CreateValidEntityDtoList(int count, long refId = 0)
        {
            var propertyImages = new List<PropertyImageDto>();

            while (propertyImages.Count < count)
            {

                var propertyImage = CreateRandomPropertyImage(refId);

                propertyImages.Add(propertyImage);

            }
            return propertyImages;
        }
        public static PropertyImageDto CreateRandomPropertyImage(long idProperty)
        {
            var owner = new PropertyImageDto()
            {
                IdProperty = idProperty,
                File = Utilities.RandomGenerators.GenerateRandomByteArray(),
                Enabled = false,
            };

            return owner;
        }
    }
}
