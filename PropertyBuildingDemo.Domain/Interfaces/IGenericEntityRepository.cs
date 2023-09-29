using PropertyBuildingDemo.Domain.Specification;
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
        IQueryable<TEntity>             Entities { get; }
        IQueryable<TEntity>             GetAll();        
        Task<TEntity>                   AddAsync(TEntity entity);
        Task<IEnumerable<TEntity>>      AddRangeAsync(IEnumerable<TEntity> entity);
        Task<TEntity>                   UpdateAsync(TEntity entity);
        Task<TEntity>                   GetAsync(long id);
        Task                            DeleteAsync(TEntity entity);

        Task<TEntity>                   FindBy(ISpecifications<TEntity> specification);
        Task<IEnumerable<TEntity>>      ListByAsync(ISpecifications<TEntity> specification);
        Task<int>                       CountAsync(ISpecifications<TEntity> specifications);
    }
}
