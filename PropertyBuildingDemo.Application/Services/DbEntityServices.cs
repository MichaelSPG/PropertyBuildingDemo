using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PropertyBuildingDemo.Application.Config;
using PropertyBuildingDemo.Application.IServices;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities.Enums;
using PropertyBuildingDemo.Domain.Interfaces;
using PropertyBuildingDemo.Domain.Specifications;
using static PropertyBuildingDemo.Application.Helpers.SpecificationEvaluator;

namespace PropertyBuildingDemo.Application.Services
{
    /// <summary>
    /// Service class for managing operations on entities of type TEntity.
    /// </summary>
    /// <typeparam name="TEntity">The entity type to be managed.</typeparam>
    /// <typeparam name="TEntityDto">The DTO (Data Transfer Object) type associated with TEntity.</typeparam>
    public class DbEntityServices<TEntity, TEntityDto> : IDbEntityServices<TEntity, TEntityDto> 
        where TEntity : BaseEntityDb
        where TEntityDto : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISystemLogger _systemLogger;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IOptions<ApplicationConfig> _appConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbEntityServices{TEntity, TEntityDto}"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work for database operations.</param>
        /// <param name="systemLogger">The system logger for logging.</param>
        /// <param name="mapper">The mapper for mapping between TEntity and TEntityDto.</param>
        /// <param name="cacheService">The cache service for caching data.</param>
        /// <param name="appConfig">The application configuration options.</param>
        public DbEntityServices(IUnitOfWork unitOfWork
            , ISystemLogger systemLogger
            , IMapper mapper, ICacheService cacheService
            , IOptions<ApplicationConfig> appConfig)
        {
            _unitOfWork = unitOfWork;
            _systemLogger = systemLogger;
            _mapper = mapper;
            _cacheService = cacheService;
            _appConfig = appConfig;
        }

        /// <summary>
        /// Retrieves cached data of the specified entity type.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{TEntity}"/> containing the cached data.</returns>
        async Task<IEnumerable<TEntity>> GetCacheData()
        {
            return await _cacheService.GetDataAsync<IEnumerable<TEntity>>(typeof(TEntity).Name);
        }

        /// <summary>
        /// Sets cached data for the specified entity type with an expiration time.
        /// </summary>
        /// <param name="entities">The data to cache.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation. Returns <c>true</c> if the data was successfully cached, otherwise <c>false</c>.</returns>
        async Task<bool> SetCacheData(IEnumerable<TEntity> entities)
        {
            var expirationTime = DateTimeOffset.Now.AddMinutes(_appConfig.Value.ExpireInMinutes);
            return await _cacheService.SetDataAsync<IEnumerable<TEntity>>(typeof(TEntity).Name, entities, expirationTime);
        }
        /// <summary>
        /// Retrieves an entity by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the entity.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation that returns the retrieved entity or null if not found.
        /// </returns>
        public async Task<TEntityDto> GetByIdAsync(long id)
        {
            var cacheData = await GetCacheData();

            if (cacheData != null)
            {
                return _mapper.Map<TEntityDto>(cacheData.ToList().Find(x => x.GetId() == id));
            }
            
            // Retrieve the entity by its ID using the unit of work and repository
            var result = await this._unitOfWork.GetRepository<TEntity>().GetAsync(id);
            
            if (result == null || result.IsDeleted)
            {
                return null;
            }
            return _mapper.Map<TEntityDto>(result);
        }

        /// <summary>
        /// Retrieves all entities of type TEntity asynchronously.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation that returns a list of TEntityDto objects.
        /// </returns>
        public async Task<List<TEntityDto>> GetAllAsync()
        {
            var cacheData = _cacheService.GetDataAsync<IEnumerable<TEntity>>(typeof(TEntity).Name).Result;

            if (cacheData != null)
            {
                return _mapper.Map<List<TEntityDto>>(cacheData.ToList());
            }
            // GEt query as no tracking
            var query = this._unitOfWork.GetRepository<TEntity>().GetAllAsNoTracking();
            // Apply where clause to query, we don want deleted fields
            query = query.Where(x => x.IsDeleted == false);
            // Executes the query
            var result = await query.ToListAsync();

            await SetCacheData(result);

            return _mapper.Map<List<TEntityDto>>(result);
        }

        /// <summary>
        /// Retrieves entities that match the specified criteria defined by the <paramref name="specifications"/> asynchronously.
        /// </summary>
        /// <param name="specifications">The specifications to filter entities.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation that returns a list of TEntityDto objects.
        /// </returns>
        public async Task<List<TEntityDto>> GetByAsync(ISpecifications<TEntity> specifications)
        {
            var cacheData = await GetCacheData();
            
            var query = cacheData != null ? cacheData.AsQueryable() : this._unitOfWork.GetRepository<TEntity>().GetAllAsNoTracking();

            query = ApplyToQuery(query, specifications);

            var result = await query.ToListAsync();

            return _mapper.Map<List<TEntityDto>>(result);
        }

        /// <summary>
        /// Updates an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation that returns the updated entity.
        /// </returns>
        public async Task<TEntityDto> UpdateAsync(TEntityDto entity)
        {
            var result = await this._unitOfWork.GetRepository<TEntity>().UpdateAsync(_mapper.Map<TEntity>(entity));
            await this._unitOfWork.Complete();
            _systemLogger.LogMessage(ELoggingLevel.Debug, $"Updated element of type: {typeof(TEntity).Name}");
            await _cacheService.RemoveDataAsync(typeof(TEntity).Name);
            return _mapper.Map<TEntityDto>(result);
        }

        /// <summary>
        /// Adds a new entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation that returns the added entity.
        /// </returns>
        public async Task<TEntityDto> AddAsync(TEntityDto entity)
        {
            var result = await this._unitOfWork.GetRepository<TEntity>().AddAsync(_mapper.Map<TEntity>(entity));
            await this._unitOfWork.Complete();
            _systemLogger.LogMessage(ELoggingLevel.Debug, $"Added element of type: {typeof(TEntity).Name}");
            await _cacheService.RemoveDataAsync(typeof(TEntity).Name);
            return _mapper.Map<TEntityDto>(result);
        }

        /// <summary>
        /// Adds a list of entities asynchronously.
        /// </summary>
        /// <param name="entity">The list of entities to add.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation that returns a list of added entities.
        /// </returns>
        public async Task<List<TEntityDto>> AddListAsync(List<TEntityDto> entity)
        {
            var result = await this._unitOfWork.GetRepository<TEntity>().AddRangeAsync(_mapper.Map<List<TEntity>>(entity));
            await this._unitOfWork.Complete();
            _systemLogger.LogMessage(ELoggingLevel.Debug, $"Added {entity.Count} element(s) of type: {typeof(TEntity).Name}");

            await _cacheService.RemoveDataAsync(typeof(TEntity).Name);
            return _mapper.Map<List<TEntityDto>>(result);
        }

        /// <summary>
        /// Deletes an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation that returns the deleted entity.
        /// </returns>
        public async Task<TEntityDto> DeleteAsync(TEntityDto entity)
        {
            if (entity == null)
            {
                throw new ArgumentException(typeof(TEntity).Name);
            }

            await this._unitOfWork.GetRepository<TEntity>().DeleteAsync(_mapper.Map<TEntity>(entity));
            await this._unitOfWork.Complete();
            _systemLogger.LogMessage(ELoggingLevel.Debug, $"Deleted element of type: {typeof(TEntity).Name}");
            await _cacheService.RemoveDataAsync(typeof(TEntity).Name);
            return _mapper.Map<TEntityDto>(entity);
        }
    }
}
