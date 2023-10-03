using AutoMapper;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PropertyBuildingDemo.Application.Config;
using PropertyBuildingDemo.Application.IServices;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities.Enums;
using PropertyBuildingDemo.Domain.Interfaces;
using PropertyBuildingDemo.Domain.Specifications;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PropertyBuildingDemo.Application.Services
{
    public class DbEntityServices<TEntity, TEntityDto> : IDbEntityServices<TEntity, TEntityDto> 
        where TEntity : BaseEntityDb
        where TEntityDto : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISystemLogger _systemLogger;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IOptions<ApplicationConfig> _appConfig;

        public  DbEntityServices(IUnitOfWork unitOfWork
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

        public async Task<TEntityDto> GetByIdAsync(long id)
        {
            var cacheData = await _cacheService.GetDataAsync<IEnumerable<TEntity>>(typeof(TEntity).Name);

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

        public async Task<List<TEntityDto>> GetAllAsync()
        {
            var cacheData = _cacheService.GetDataAsync<IEnumerable<TEntity>>(typeof(TEntity).Name).Result;

            if (cacheData != null)
            {
                return _mapper.Map<List<TEntityDto>>(cacheData.ToList());
            }

            var result = await this._unitOfWork.GetRepository<TEntity>().GetAll().Where(x => x.IsDeleted == false).ToListAsync(); 
            var expirationTime = DateTimeOffset.Now.AddMinutes(_appConfig.Value.ExpireInMinutes);
            await _cacheService.SetDataAsync<IEnumerable<TEntity>>(typeof(TEntity).Name, result, expirationTime).ConfigureAwait(false);
            return _mapper.Map<List<TEntityDto>>(result);
        }

        public async Task<List<TEntityDto>> GetByAsync(ISpecifications<TEntity> specifications)
        {
            var cacheData = await _cacheService.GetDataAsync<IEnumerable<TEntity>>(typeof(TEntity).Name);

            if (cacheData != null)
            {
                cacheData = Application.Helpers.SpecificationEvaluator.ApplyToQuery(cacheData.AsQueryable(), specifications).ToList();

                return _mapper.Map<List<TEntityDto>>(cacheData);
            }

            var result = await this._unitOfWork.GetRepository<TEntity>()
                .ListByAsync(specifications);

            return _mapper.Map<List<TEntityDto>>(result);
        }

        public async Task<TEntityDto> UpdateAsync(TEntityDto entity)
        {
            var result = await this._unitOfWork.GetRepository<TEntity>().UpdateAsync(_mapper.Map<TEntity>(entity));
            await this._unitOfWork.Complete();
            _systemLogger.LogMessage(ELoggingLevel.Debug, $"Updated element of type: {typeof(TEntity).Name}");
            await _cacheService.RemoveDataAsync(typeof(TEntity).Name);
            return _mapper.Map<TEntityDto>(result);
        }

        public async Task<TEntityDto> AddAsync(TEntityDto entity)
        {
            var result = await this._unitOfWork.GetRepository<TEntity>().AddAsync(_mapper.Map<TEntity>(entity));
            await this._unitOfWork.Complete();
            _systemLogger.LogMessage(ELoggingLevel.Debug, $"Added element of type: {typeof(TEntity).Name}");
            await _cacheService.RemoveDataAsync(typeof(TEntity).Name);
            return _mapper.Map<TEntityDto>(result);
        }

        public async Task<List<TEntityDto>> AddListAsync(List<TEntityDto> entity)
        {
            var result = await this._unitOfWork.GetRepository<TEntity>().AddRangeAsync(_mapper.Map<List<TEntity>>(entity));
            await this._unitOfWork.Complete();
            _systemLogger.LogMessage(ELoggingLevel.Debug, $"Added {entity.Count} element(s) of type: {typeof(TEntity).Name}");

            await _cacheService.RemoveDataAsync(typeof(TEntity).Name);
            return _mapper.Map<List<TEntityDto>>(result);
        }

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
