using PropertyBuildingDemo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Domain.Specification
{
    /// <summary>
    /// INTERFACE: Used for applying filters with specific criteria, orderings and pagination
    /// OR want the result in the form of pagination then you need to add the specification class
    /// </summary>
    /// <typeparam name="TEntity">The entity object type</typeparam>
    public interface ISpecifications<TEntity> where TEntity : class, IEntityDB
    {
        Expression<Func<TEntity, bool>>           Criteria { get;}
        List<Expression<Func<TEntity, object>>>   Includes{ get; }
        Expression<Func<TEntity, object>>         OrderBy { get; }
        Expression<Func<TEntity, object>>         OrderByDescending{ get; }
        Expression<Func<TEntity, bool>>           And(Expression<Func<TEntity, bool>> query);
        Expression<Func<TEntity, bool>>           Or(Expression<Func<TEntity, bool>> query);
        int                                       Take { get; }
        int                                       Skip { get; }
        bool                                      IsPagingEnabled { get; }

    }
}
