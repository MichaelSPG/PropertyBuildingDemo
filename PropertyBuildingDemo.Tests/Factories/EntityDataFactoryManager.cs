using PropertyBuildingDemo.Application.Dto;

namespace PropertyBuildingDemo.Tests.Factories
{
    /// <summary>
    /// A static class that manages data factories for different entity types.
    /// </summary>
    public static class EntityDataFactoryManager
    {
        private static readonly Dictionary<Type, object> Factories = new Dictionary<Type, object>();

        /// <summary>
        /// Private constructor to prevent external instantiation.
        /// </summary>
        static EntityDataFactoryManager()
        {
            // Register data factories for different types here.
            Factories[typeof(OwnerDto)] = new OwnerDataFactory();
            Factories[typeof(PropertyImageDto)] = new PropertyImageDataFactory();
            Factories[typeof(PropertyTraceDto)] = new PropertyTraceDataFactory();
            Factories[typeof(PropertyDto)] = new PropertyDataFactory();
            // Add more registrations for other entity types as needed.
        }

        /// <summary>
        /// Gets a data factory for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity for which to get the data factory.</typeparam>
        /// <returns>A data factory for the specified entity type.</returns>
        /// <exception cref="NotSupportedException">Thrown if a data factory for the specified entity type is not supported.</exception>
        public static IDataFactory<TEntity> GetFactory<TEntity>()
        {
            if (Factories.TryGetValue(typeof(TEntity), out object factory))
            {
                return (IDataFactory<TEntity>)factory;
            }

            throw new NotSupportedException($"Data factory for {typeof(TEntity)} is not supported.");
        }
    }
}
