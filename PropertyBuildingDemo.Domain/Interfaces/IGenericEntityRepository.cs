using PropertyBuildingDemo.Domain.Specifications;

namespace PropertyBuildingDemo.Domain.Interfaces
{
    /// <summary>
    /// Represents a repository interface for implementing the Repository pattern.
    /// </summary>
    /// <typeparam name="TEntity">The entity (representation of a table object).</typeparam>
    public interface IGenericEntityRepository<TEntity> where TEntity : class, IEntityDb
    {
        /// <summary>
        /// Gets a queryable collection of entities.
        /// </summary>
        IQueryable<TEntity> Entities { get; }

        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>A queryable collection of all entities.</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Adds an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity.</returns>
        Task<TEntity> AddAsync(TEntity entity);

        /// <summary>
        /// Adds a collection of entities asynchronously.
        /// </summary>
        /// <param name="entity">The collection of entities to add.</param>
        /// <returns>The added entities.</returns>
        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entity);

        /// <summary>
        /// Updates an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>The updated entity.</returns>
        Task<TEntity> UpdateAsync(TEntity entity);

        /// <summary>
        /// Gets an entity by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>The retrieved entity, or null if not found.</returns>
        Task<TEntity> GetAsync(long id);

        /// <summary>
        /// Deletes an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Finds an entity by a specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification to filter the entity.</param>
        /// <returns>The retrieved entity, or null if not found.</returns>
        Task<TEntity> FindBy(ISpecifications<TEntity> specification);

        /// <summary>
        /// Lists entities based on a specification asynchronously.
        /// </summary>
        /// <param name="specification">The specification to filter the entities.</param>
        /// <returns>A queryable collection of entities that match the specification.</returns>
        Task<IEnumerable<TEntity>> ListByAsync(ISpecifications<TEntity> specification);

        /// <summary>
        /// Counts entities based on a specification asynchronously.
        /// </summary>
        /// <param name="specifications">The specification to filter the entities.</param>
        /// <returns>The count of entities that match the specification.</returns>
        Task<int> CountAsync(ISpecifications<TEntity> specifications);
    }
}
