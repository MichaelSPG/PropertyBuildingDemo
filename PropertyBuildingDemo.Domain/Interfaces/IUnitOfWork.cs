using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericEntityRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntityDB;
        Task<bool> Complete(CancellationToken cancellationToken = default);
    }
}
