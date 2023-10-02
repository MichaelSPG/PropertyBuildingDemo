using PropertyBuildingDemo.Application.Dto;

namespace PropertyBuildingDemo.Tests.Factories
{
    static class EntityDataFactory
    {
        private static readonly Dictionary<Type, object> Factories = new Dictionary<Type, object>();

        // Private constructor to prevent external instantiation.
        static EntityDataFactory()
        {
            // Register your data factories for different types here.
            Factories[typeof(OwnerDto)] = new OwnerDataFactory();
            Factories[typeof(PropertyImageDto)] = new PropertyImageDataFactory();
            Factories[typeof(PropertyTraceDto)] = new PropertyTraceDataFactory();
            Factories[typeof(PropertyDto)] = new PropertyDataFactory();
            // Add more registrations for other entity types as needed.
        }

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
