﻿using Microsoft.EntityFrameworkCore;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Specifications;

namespace PropertyBuildingDemo.Application.Helpers
{
    /// <summary>
    /// Class to apply the specifications
    /// </summary>
    /// <typeparam name="TEntity">The entity of type BaseEntityDB</typeparam>
    public static class SpecificationEvaluator
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
        public static IQueryable<TEntity> ApplyToQuery<TEntity>(IQueryable<TEntity> inputQuery, ISpecifications<TEntity> spec) where TEntity : BaseEntityDb
        {
            var query = inputQuery.AsQueryable();
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }
            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
            return query;
        }
    }
}
