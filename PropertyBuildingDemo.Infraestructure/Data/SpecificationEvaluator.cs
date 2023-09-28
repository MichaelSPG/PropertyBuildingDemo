using Microsoft.EntityFrameworkCore;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Infrastructure.Data
{
    /// <summary>
    /// Class to apply the specifications
    /// </summary>
    /// <typeparam name="TEntity">The entity of type BaseEntityDB</typeparam>
    public class SpecificationEvaluator<TEntity>  where TEntity : BaseEntityDB
    {
        /// <summary>
        /// Applies the specifications to an input query and returns the results
        /// </summary>
        /// <remarks>
        /// If IsPagingEnabled will skip an take the results based on the parameters
        /// </remarks>
        /// <param name="inputQuery">The input query to apply specifications</param>
        /// <param name="spec">The specifications</param>
        /// <returns>the result query with applied specifications</returns>
        public static IQueryable<TEntity> ApplyToQueryQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity> spec)
        {
            var Query = inputQuery.AsQueryable();
            if (spec.Criteria != null)
            {
                Query = Query.Where(spec.Criteria);
            }
            if (spec.OrderBy != null)
            {
                Query = Query.OrderBy(spec.OrderBy);
            }
            if (spec.OrderByDescending != null)
            {
                Query = Query.OrderByDescending(spec.OrderByDescending);
            }
            if (spec.IsPagingEnabled)
            {
                Query = Query.Skip(spec.Skip).Take(spec.Take);
            }
            Query = spec.Includes.Aggregate(Query, (current, include) => current.Include(include));
            return Query;
        }
    }
}
