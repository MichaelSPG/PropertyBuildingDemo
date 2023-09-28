using Microsoft.EntityFrameworkCore;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Interfaces;
using PropertyBuildingDemo.Domain.Specification;
using PropertyBuildingDemo.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Infrastructure.Repositories
{
    public class BaseEntityRepository<TEntity> : IGenericEntityRepository<TEntity> where TEntity : BaseEntityDB
    {
        private readonly PropertyBuildingContext _context;

        public BaseEntityRepository(PropertyBuildingContext context)
        {
            _context = context;
        }

        public IQueryable<TEntity> Entities => _context.Set<TEntity>();

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            entity.UpdatedTime = DateTime.Now;
            entity.CreatedTime = DateTime.Now;
            await _context.Set<TEntity>().AddAsync(entity);
            return entity;
        }

        public Task DeleteAsync(TEntity entity)
        {
            entity.UpdatedTime = DateTime.Now;
            entity.IsDeleted = true;
            _context.Set<TEntity>().Update(entity);
            return Task.CompletedTask;
        }

        public IQueryable<TEntity> GetAllAsync()
        {
            return _context.Set<TEntity>().AsQueryable();
        }

        public async Task<TEntity> GetAsync(long id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> FindBy(ISpecifications<TEntity> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TEntity>> ListByAsync(ISpecifications<TEntity> specification)
        {
            return await ApplySpecification(specification).ToListAsync();
        }
        public async Task<int> CountAsync(ISpecifications<TEntity> specifications)
        {
            return await ApplySpecification(specifications).CountAsync();
        }

        private IQueryable<TEntity> ApplySpecification(ISpecifications<TEntity> specifications)
        {
            return SpecificationEvaluator<TEntity>.ApplyToQueryQuery(_context.Set<TEntity>().AsQueryable(), specifications);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            await Task.Delay(1);
            entity.UpdatedTime = DateTime.Now;
            _context.Set<TEntity>().Update(entity);
            return entity;
        }
    }
}
