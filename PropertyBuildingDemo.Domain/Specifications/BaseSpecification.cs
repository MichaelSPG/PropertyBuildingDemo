using LinqKit;
using PropertyBuildingDemo.Domain.Interfaces;
using System.Linq.Expressions;

namespace PropertyBuildingDemo.Domain.Specifications
{
    /// <summary>
    /// Represents a base specification for querying entities.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity for the specification.</typeparam>
    public class BaseSpecifications<TEntity> : ISpecifications<TEntity> where TEntity : class, IEntityDb
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSpecifications{TEntity}"/> class.
        /// </summary>
        public BaseSpecifications() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseSpecifications{TEntity}"/> class with a criteria expression.
        /// </summary>
        /// <param name="inCriteria">The criteria expression for the specification.</param>
        public BaseSpecifications(Expression<Func<TEntity, bool>> inCriteria)
        {
            this.Criteria = inCriteria;
        }

        /// <summary>
        /// Gets or sets the criteria expression for the specification.
        /// </summary>
        public Expression<Func<TEntity, bool>> Criteria { get; set; }

        /// <summary>
        /// Gets a list of include expressions for related entities.
        /// </summary>
        public List<Expression<Func<TEntity, object>>> Includes { get; } = new List<Expression<Func<TEntity, object>>>();

        /// <summary>
        /// Gets or sets the order by expression.
        /// </summary>
        public Expression<Func<TEntity, object>> OrderBy { get; private set; }

        /// <summary>
        /// Gets or sets the order by descending expression.
        /// </summary>
        public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

        /// <summary>
        /// Gets or sets the number of records to take (for paging).
        /// </summary>
        public int Take { get; private set; }

        /// <summary>
        /// Gets or sets the number of records to skip (for paging).
        /// </summary>
        public int Skip { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether paging is enabled.
        /// </summary>
        public bool IsPagingEnabled { get; private set; }

        /// <summary>
        /// Adds an include expression for related entities.
        /// </summary>
        /// <param name="inIncludeExpression">The include expression to add.</param>
        protected void AddInclude(Expression<Func<TEntity, object>> inIncludeExpression)
        {
            Includes.Add(inIncludeExpression);
        }

        /// <summary>
        /// Adds an order by expression.
        /// </summary>
        /// <param name="inOrderByExpression">The order by expression to add.</param>
        public void AddOrderBy(Expression<Func<TEntity, object>> inOrderByExpression)
        {
            OrderBy = inOrderByExpression;
        }

        /// <summary>
        /// Adds an order by descending expression.
        /// </summary>
        /// <param name="inOrderByDescendingExpression">The order by descending expression to add.</param>
        public void AddOrderByDescending(Expression<Func<TEntity, object>> inOrderByDescendingExpression)
        {
            OrderByDescending = inOrderByDescendingExpression;
        }

        /// <summary>
        /// Applies paging to the specification.
        /// </summary>
        /// <param name="inTake">The number of records to take.</param>
        /// <param name="inSkip">The number of records to skip.</param>
        public void ApplyingPaging(int inTake, int inSkip)
        {
            this.Take = inTake;
            this.Skip = inSkip;
            this.IsPagingEnabled = true;
        }

        /// <summary>
        /// Combines the current specification with an AND operation.
        /// </summary>
        /// <param name="inQuery">The query to combine with.</param>
        /// <returns>The combined specification.</returns>
        public Expression<Func<TEntity, bool>> And(Expression<Func<TEntity, bool>> inQuery)
        {
            return Criteria = Criteria == null ? inQuery : Criteria.And(inQuery);
        }

        /// <summary>
        /// Combines the current specification with an OR operation.
        /// </summary>
        /// <param name="inQuery">The query to combine with.</param>
        /// <returns>The combined specification.</returns>
        public Expression<Func<TEntity, bool>> Or(Expression<Func<TEntity, bool>> inQuery)
        {
            return Criteria = Criteria == null ? inQuery : Criteria.Or(inQuery);
        }
    }
}
