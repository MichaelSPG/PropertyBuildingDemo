using PropertyBuildingDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Domain.Interfaces
{
    public interface IEntityService<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity>   GetAsync(long id);
        Task<TEntity>   AddAsync(TEntity entity);
        Task<TEntity>   UpdateAsync(TEntity entity);
        Task<bool>      DeleteAsync(TEntity entity);
    }
}
