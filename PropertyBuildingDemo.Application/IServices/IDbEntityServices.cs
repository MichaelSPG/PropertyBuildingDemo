using PropertyBuildingDemo.Domain.Interfaces;
using PropertyBuildingDemo.Domain.Specifications;

namespace PropertyBuildingDemo.Application.IServices
{
    public interface IDbEntityServices<TEntity, TEntityDto> 
        where TEntity : class, IEntityDb 
        where TEntityDto : class
    {
        Task<TEntityDto> GetByIdAsync(long id);
        Task<List<TEntityDto>> GetAllAsync();
        Task<List<TEntityDto>> GetByAsync(ISpecifications<TEntity> specifications);
        Task<TEntityDto> UpdateAsync(TEntityDto entity);
        Task<TEntityDto> AddAsync(TEntityDto entity);
        Task<List<TEntityDto>> AddListAsync(List<TEntityDto> entity);
        Task<TEntityDto> DeleteAsync(TEntityDto entity);
    }
}
