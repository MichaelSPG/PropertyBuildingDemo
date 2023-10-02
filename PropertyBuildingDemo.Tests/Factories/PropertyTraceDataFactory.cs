using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Tests.Helpers;

namespace PropertyBuildingDemo.Tests.Factories
{
    public class PropertyTraceDataFactory : IDataFactory<PropertyTraceDto>
    {
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
            };
        }

        public IEnumerable<PropertyTraceDto> CreateValidEntityDtoList(int count, long refId = 0)
        {
            var propertyImages = new List<PropertyTraceDto>();

            while (propertyImages.Count < count)
            {
                var propertyImage = CreateRandomPropertyTrace(refId);

                propertyImages.Add(propertyImage);

            }
            return propertyImages;
        }
        public static PropertyTraceDto CreateRandomPropertyTrace(long idProperty)
        {
            var owner = new PropertyTraceDto()
            {
                IdProperty = 0,
                Value = (decimal)Utilities.Random.NextDouble() * 1000,
                DateSale = DateTime.Now.AddDays(-Utilities.Random.Next(40, 2000)),
                Tax = (decimal)Utilities.Random.NextDouble() * 1000,
            };

            return owner;
        }
    }
}
