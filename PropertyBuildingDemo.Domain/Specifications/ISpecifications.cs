using PropertyBuildingDemo.Domain.Interfaces;
using System.Linq.Expressions;

namespace PropertyBuildingDemo.Domain.Specifications
{

    /// <summary>
    /// Represents an interface for specifying criteria, orderings, and pagination for querying entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity for the specification.</typeparam>
    public interface ISpecifications<TEntity> where TEntity : class, IEntityDb
    {
        /// <summary>
        /// Gets the criteria expression for the specification.
        /// </summary>
        Expression<Func<TEntity, bool>> Criteria { get; }

        /// <summary>
        /// Gets a list of include expressions for related entities.
        /// </summary>
        List<Expression<Func<TEntity, object>>> Includes { get; }

        /// <summary>
        /// Gets the order by expression.
        /// </summary>
        Expression<Func<TEntity, object>> OrderBy { get; }

        /// <summary>
        /// Gets the order by descending expression.
        /// </summary>
        Expression<Func<TEntity, object>> OrderByDescending { get; }

        /// <summary>
        /// Combines the current specification with an AND operation.
        /// </summary>
        /// <param name="query">The query to combine with.</param>
        /// <returns>The combined specification.</returns>
        Expression<Func<TEntity, bool>> And(Expression<Func<TEntity, bool>> query);

        /// <summary>
        /// Combines the current specification with an OR operation.
        /// </summary>
        /// <param name="query">The query to combine with.</param>
        /// <returns>The combined specification.</returns>
        Expression<Func<TEntity, bool>> Or(Expression<Func<TEntity, bool>> query);

        /// <summary>
        /// Gets the number of records to take (for paging).
        /// </summary>
        int Take { get; }

        /// <summary>
        /// Gets the number of records to skip (for paging).
        /// </summary>
        int Skip { get; }

        /// <summary>
        /// Gets a value indicating whether paging is enabled.
        /// </summary>
        bool IsPagingEnabled { get; }
    }
}
