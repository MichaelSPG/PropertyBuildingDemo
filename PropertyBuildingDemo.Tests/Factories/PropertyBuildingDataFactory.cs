using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Tests.Helpers;

namespace PropertyBuildingDemo.Tests.Factories
{
    public static class PropertyBuildingDataFactory
    {
        public static IDataFactory<OwnerDto> OwnerDataFactory;
        public static IDataFactory<PropertyDto> PropertyDataFactory;
        public static IDataFactory<PropertyTraceDto> PropertyTraceDataFactory;
        public static IDataFactory<PropertyImageDto> PropertyImageDataFactory;
        static PropertyBuildingDataFactory()
        {
            OwnerDataFactory = EntityDataFactoryManager.GetFactory<OwnerDto>();
            PropertyDataFactory = EntityDataFactoryManager.GetFactory<PropertyDto>();
            PropertyTraceDataFactory = EntityDataFactoryManager.GetFactory<PropertyTraceDto>();
            PropertyImageDataFactory = EntityDataFactoryManager.GetFactory<PropertyImageDto>();
        }

        public static List<PropertyDto> GenerateRandomValidProperties(int count,  List<long> validOwnerIds, long ownerId = 0, bool sameOwner = false, int minImages = 0, int minTraces = 0)
        {
            var propertyList = PropertyDataFactory.CreateValidEntityDtoList(count);
            if (ownerId <= 0)
            {
                ownerId = validOwnerIds[Utilities.Random.Next(0, validOwnerIds.Count)];
            }

            foreach (var property in propertyList)
            {
                if (minImages > 0)
                {
                    property.PropertyImages = PropertyImageDataFactory.CreateValidEntityDtoList(minImages);
                }

                if (minTraces > 0)
                {
                    property.PropertyTraces = PropertyTraceDataFactory.CreateValidEntityDtoList(minTraces);
                }

                if (!sameOwner)
                {
                    ownerId = validOwnerIds[Utilities.Random.Next(0, validOwnerIds.Count)];
                }
                property.IdOwner = ownerId;
            }
            return propertyList.ToList();
        }
    }
}
