using PropertyBuildingDemo.Domain.Interfaces;
using PropertyBuildingDemo.Infrastructure.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PropertyBuildingDemo.Domain.Entities.Enums;
using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using PropertyBuildingDemo.Application.IServices;
using Microsoft.Extensions.Options;
using PropertyBuildingDemo.Application.Config;

namespace PropertyBuildingDemo.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {        
        private readonly PropertyBuildingContext    _context;
        private readonly ISystemLogger              _systemLogger;
        private readonly ICacheService _cacheService;
        private readonly IOptions<ApplicationConfig> _appOptions;
        private Hashtable                           _repositories;
        private IDbContextTransaction               _transaction;
        public UnitOfWork(PropertyBuildingContext context, ISystemLogger systemLogger, ICacheService cacheService, IOptions<ApplicationConfig> appOptions)
        {
            _context = context;
            this._systemLogger = systemLogger;
            _cacheService = cacheService;
            _appOptions = appOptions;
        }

        public async Task BeginTransaction(CancellationToken cancellationToken = default)
        {
            _transaction ??= await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task<bool> Complete(CancellationToken cancellationToken = default, bool inFinishTransaction = true)
        {
            var result = false;
            int rowsAffected = -1;
            try
            {
                rowsAffected = await _context.SaveChangesAsync(cancellationToken);
                
                if (_transaction != null && inFinishTransaction)
                    await _transaction.CommitAsync(cancellationToken);
                result = rowsAffected > 0;
            }
            catch (Exception exc)
            {
                _systemLogger.LogExceptionMessage(ELoggingLevel.Error, this, exc);
                if (_transaction != null)
                    await _transaction.RollbackAsync(cancellationToken);
                throw exc.InnerException?? exc;
            }
            finally
            {
                if (_transaction != null && inFinishTransaction)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
            }
            return result;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        IGenericEntityRepository<TEntity> IUnitOfWork.GetRepository<TEntity>()
        {
            if (_repositories == null) _repositories = new Hashtable();
            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositiryType = typeof(BaseEntityRepository<>);
                var repositoryInstance = Activator.CreateInstance( repositiryType.MakeGenericType(typeof(TEntity)), _context);
                _repositories.Add(type, repositoryInstance);
            }
            return (IGenericEntityRepository<TEntity>)_repositories[type];
        }
    }
}
