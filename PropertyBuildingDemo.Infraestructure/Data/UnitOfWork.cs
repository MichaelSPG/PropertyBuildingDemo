using PropertyBuildingDemo.Domain.Interfaces;
using PropertyBuildingDemo.Infrastructure.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {        
        private readonly PropertyBuildingContext    _context;
        private readonly ISystemLogger              systemLogger;
        private Hashtable                           _repositories;

        public UnitOfWork(PropertyBuildingContext context, ISystemLogger systemLogger)
        {
            _context = context;
            this.systemLogger = systemLogger;
        }
        public async Task<bool> Complete(CancellationToken cancellationToken = default)
        {
            var result = false;
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                result = true;
            }
            catch (Exception exc)
            {
                systemLogger.LogExceptionMessage(Domain.Entities.Enums.eLogginLevel.Level_Error, this, exc);
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
            var Type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(Type))
            {
                var repositiryType = typeof(BaseEntityRepository<>);
                var repositoryInstance = Activator.CreateInstance( repositiryType.MakeGenericType(typeof(TEntity)), _context);
                _repositories.Add(Type, repositoryInstance);
            }
            return (IGenericEntityRepository<TEntity>)_repositories[Type];
        }
    }
}
