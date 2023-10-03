using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PropertyBuildingDemo.Application.Config;
using PropertyBuildingDemo.Application.IServices;
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
        private readonly ICacheService _cacheService;
        private readonly IOptions<ApplicationConfig> _appOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseEntityRepository{TEntity}"/> class.
        /// </summary>
        /// <param name="context">The <see cref="PropertyBuildingContext"/> instance.</param>
        public BaseEntityRepository(PropertyBuildingContext context, ICacheService cacheService, IOptions<ApplicationConfig> appOptions)
        {
            _context = context;
            _cacheService = cacheService;
            _appOptions = appOptions;
        }

        /// <summary>
        /// Gets a queryable collection of entities.
        /// </summary>
        public IQueryable<TEntity> Entities => _context.Set<TEntity>();

        /// <summary>
        /// Gets all entities from the repository.
        /// </summary>
        /// <returns>A queryable collection of entities.</returns>
        public IQueryable<TEntity> GetAll()
        {
            var cacheData = _cacheService.GetDataAsync<IEnumerable<TEntity>>(typeof(TEntity).Name).Result;

            if (cacheData != null)
            {
                return cacheData.AsQueryable();
            }

            var result = Entities;
            var expirationTime = DateTimeOffset.Now.AddMinutes(_appOptions.Value.ExpireInMinutes);
            _cacheService.SetDataAsync<IEnumerable<TEntity>>(typeof(TEntity).Name, result.ToList(), expirationTime).ConfigureAwait(false);
            return result;
        }

        /// <summary>
        /// Gets an entity by its ID from the repository.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>The retrieved entity.</returns>
        public async Task<TEntity> GetAsync(long id)
        {
            var cacheData = await _cacheService.GetDataAsync<IEnumerable<TEntity>>(typeof(TEntity).Name);

            if (cacheData != null)
            {
                return cacheData.ToList().Find(x => x.GetId() == id);
            }

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
            var cacheData = _cacheService.GetDataAsync<IEnumerable<TEntity>>(typeof(TEntity).Name).Result;

            var query = cacheData != null ? cacheData.AsQueryable() : _context.Set<TEntity>().AsQueryable();
            return Application.Helpers.SpecificationEvaluator.ApplyToQuery(query, specifications);
        }

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
            await _cacheService.RemoveDataAsync(typeof(TEntity).Name);
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
            await _cacheService.RemoveDataAsync(typeof(TEntity).Name);
            return entities;
        }

        /// <summary>
        /// Marks an entity as deleted in the repository.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>A completed <see cref="Task"/>.</returns>
        public async Task DeleteAsync(TEntity entity)
        {
            UntrackEntity(entity);
            _context.Entry(entity).State = EntityState.Modified;
            entity.UpdatedTime = DateTime.Now;
            entity.IsDeleted = true;
            _context.Set<TEntity>().Update(entity);
            await _cacheService.RemoveDataAsync(typeof(TEntity).Name);
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

            entity.UpdatedTime = DateTime.Now;
            _context.Set<TEntity>().Update(entity);
            await _cacheService.RemoveDataAsync(typeof(TEntity).Name);
            return entity;
        }
    }
}
