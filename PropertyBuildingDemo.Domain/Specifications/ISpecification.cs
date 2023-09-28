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
    /// <typeparam name="T">The entity object type</typeparam>
    public interface ISpecification<T> where T : class, IEntityDB
    {
        Expression<Func<T, bool>>           Criteria { get;}
        List<Expression<Func<T, object>>>   Includes{ get; }
        Expression<Func<T, object>>         OrderBy { get; }
        Expression<Func<T, object>>         OrderByDescending{ get; }
        int                                 Take { get; }
        int                                 Skip { get; }
        bool                                IsPagingEnabled { get; }

    }
}
