using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Domain.Interfaces
{
    /// <summary>
    /// Repository interface / for implementing Repository pattern
    /// </summary>
    /// <typeparam name="TEntity">The entity (representation of table object) </typeparam>
    public interface IGenericEntityRepository<TEntity> where TEntity : class, IEntityDB 
    {
        IQueryable<TEntity> GetAllAsync();        
        Task<TEntity>   AddAsync(TEntity entity);
        Task<TEntity>   UpdateAsync(TEntity entity);
        Task<TEntity>   GetAsync(long id);
        Task<bool>      DeleteAsync(long id);
    }
}
