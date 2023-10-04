using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Tests.Helpers;

namespace PropertyBuildingDemo.Tests.Factories
{
    /// <summary>
    /// This static class serves as a factory for creating random valid properties with associated owners, images, and traces.
    /// </summary>
    public static class PropertyBuildingDataFactory
    {
        /// <summary>
        /// Gets or sets the data factory for creating valid OwnerDto instances.
        /// </summary>
        public static IDataFactory<OwnerDto> OwnerDataFactory { get; set; }

        /// <summary>
        /// Gets or sets the data factory for creating valid PropertyDto instances.
        /// </summary>
        public static IDataFactory<PropertyDto> PropertyDataFactory { get; set; }

        /// <summary>
        /// Gets or sets the data factory for creating valid PropertyTraceDto instances.
        /// </summary>
        public static IDataFactory<PropertyTraceDto> PropertyTraceDataFactory { get; set; }

        /// <summary>
        /// Gets or sets the data factory for creating valid PropertyImageDto instances.
        /// </summary>
        public static IDataFactory<PropertyImageDto> PropertyImageDataFactory { get; set; }

        /// <summary>
        /// Static constructor that initializes the data factories by obtaining them from the EntityDataFactoryManager.
        /// </summary>
        static PropertyBuildingDataFactory()
        {
            OwnerDataFactory = EntityDataFactoryManager.GetFactory<OwnerDto>();
            PropertyDataFactory = EntityDataFactoryManager.GetFactory<PropertyDto>();
            PropertyTraceDataFactory = EntityDataFactoryManager.GetFactory<PropertyTraceDto>();
            PropertyImageDataFactory = EntityDataFactoryManager.GetFactory<PropertyImageDto>();
        }

        /// <summary>
        /// Generates a list of random valid properties with associated owners, images, and traces.
        /// </summary>
        /// <param name="count">The number of properties to generate.</param>
        /// <param name="validOwnerIds">A list of valid owner IDs.</param>
        /// <param name="ownerId">The owner ID for the properties (optional).</param>
        /// <param name="sameOwner">Indicates whether all properties have the same owner (optional).</param>
        /// <param name="minImages">The minimum number of images for each property (optional).</param>
        /// <param name="minTraces">The minimum number of traces for each property (optional).</param>
        /// <param name="imageBytesCount">The byte count for property images (optional).</param>
        /// <returns>A list of random valid PropertyDto instances.</returns>
        public static List<PropertyDto> GenerateRandomValidProperties(
            int count, List<long> validOwnerIds, long ownerId = 0, bool sameOwner = false, int minImages = 0, int minTraces = 0, int imageBytesCount = 0)
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
                    property.PropertyImages = PropertyImageDataFactory.CreateValidEntityDtoList(minImages, param: imageBytesCount);
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
