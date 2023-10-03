using Microsoft.EntityFrameworkCore;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Interfaces;
using PropertyBuildingDemo.Domain.Specifications;
using PropertyBuildingDemo.Infrastructure.Data;
using EntityState = Microsoft.EntityFrameworkCore.EntityState;

namespace PropertyBuildingDemo.Infrastructure.Repositories
{
    /// <summary>
    /// Represents a base repository for entities that provides common CRUD operations.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    public class BaseEntityRepository<TEntity> : IGenericEntityRepository<TEntity> where TEntity : BaseEntityDb
    {
        private readonly PropertyBuildingContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEntityRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="context">The <see cref="PropertyBuildingContext"/> instance.</param>
        public BaseEntityRepository(PropertyBuildingContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a queryable collection of entities.
        /// </summary>
        public IQueryable<TEntity> Entities => _context.Set<TEntity>();

        /// <summary>
        /// Adds a new entity to the repository.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The added entity.</returns>
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            entity.UpdatedTime = DateTime.Now;
            entity.CreatedTime = DateTime.Now;
            await _context.Set<TEntity>().AddAsync(entity);
            return entity;
        }

        /// <summary>
        /// Adds a collection of entities to the repository.
        /// </summary>
        /// <param name="entity">The collection of entities to add.</param>
        /// <returns>The added entities.</returns>
        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entity)
        {
            IEnumerable<TEntity> entities = entity as TEntity[] ?? entity.ToArray();
            foreach (var baseEntityDb in entities)
            {
                baseEntityDb.CreatedTime = baseEntityDb.UpdatedTime = DateTime.Now;
            }
            await _context.Set<TEntity>().AddRangeAsync(entities);
            return entities;
        }

        /// <summary>
        /// Marks an entity as deleted in the repository.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A completed <see cref="Task"/>.</returns>
        public Task DeleteAsync(TEntity entity)
        {
            UntrackEntity(entity);
            _context.Entry(entity).State = EntityState.Modified;
            entity.UpdatedTime = DateTime.Now;
            entity.IsDeleted = true;
            _context.Set<TEntity>().Update(entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets all entities from the repository.
        /// </summary>
        /// <returns>A queryable collection of entities.</returns>
        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        /// <summary>
        /// Gets an entity by its ID from the repository.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>The retrieved entity.</returns>
        public async Task<TEntity> GetAsync(long id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        /// <summary>
        /// Finds an entity based on the provided specification.
        /// </summary>
        /// <param name="specification">The specification to apply.</param>
        /// <returns>The found entity.</returns>
        public async Task<TEntity> FindBy(ISpecifications<TEntity> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Lists entities based on the provided specification.
        /// </summary>
        /// <param name="specification">The specification to apply.</param>
        /// <returns>A collection of matching entities.</returns>
        public async Task<IEnumerable<TEntity>> ListByAsync(ISpecifications<TEntity> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }

        /// <summary>
        /// Counts entities based on the provided specification.
        /// </summary>
        /// <param name="specifications">The specification to apply.</param>
        /// <returns>The count of matching entities.</returns>
        public async Task<int> CountAsync(ISpecifications<TEntity> specifications)
        {
            return await ApplySpecification(specifications).CountAsync();
        }

        private IQueryable<TEntity> ApplySpecification(ISpecifications<TEntity> specifications)
        {
            //UntrackEntity(entity);
            return SpecificationEvaluator<TEntity>.ApplyToQuery(_context.Set<TEntity>().AsQueryable(), specifications);
        }

        private void UntrackEntity(TEntity entity)
        {

            var local = _context.Set<TEntity>()
                .Local
                .FirstOrDefault(entry => entry.GetId().Equals(entity.GetId()));
            // check if local is not null 
            if (local != null)
            {
                // detach
                _context.Entry(local).State = EntityState.Detached;
            }
        }

        /// <summary>
        /// Updates an entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>The updated entity.</returns>
        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            await Task.Delay(1); // Placeholder for update logic
            UntrackEntity(entity);
            _context.Entry(entity).State = EntityState.Modified;
            //entity = _context.Set<TEntity>().Local.FirstOrDefault(entry => entry.GetId() == entity.GetId());
            //if (entity != null)
            //{
            //    _context.Entry(entity).State = EntityState.Detached;
            //}
            //_context.Entry(entity).State = EntityState.Modified;

            entity.UpdatedTime = DateTime.Now;
            _context.Set<TEntity>().Update(entity);
            
            return entity;
        }
    }
}
