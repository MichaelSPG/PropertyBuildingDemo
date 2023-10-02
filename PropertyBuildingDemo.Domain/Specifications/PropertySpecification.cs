using PropertyBuildingDemo.Domain.Entities;
using System.Linq.Expressions;

namespace PropertyBuildingDemo.Domain.Specifications
{
    /// <summary>
    /// Represents a specification for querying properties with specific criteria, orderings, and pagination.
    /// </summary>
    public class PropertySpecification : BaseSpecifications<Property>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySpecification"/> class with a filter based on owner ID.
        /// </summary>
        /// <param name="inIdOwner">The owner ID to filter by.</param>
        public PropertySpecification(long inIdOwner)
            : base(x => x.IdOwner == inIdOwner)
        {
            AddInclude(o => o.Owner);
            AddOrderByDescending(od => od.IdProperty);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySpecification"/> class with multiple filters.
        /// </summary>
        /// <param name="inFilters">A list of filter expressions.</param>
        public PropertySpecification(List<Expression<Func<Property, bool>>> inFilters, List<Expression<Func<Property, object>>> includes)
            : base(x => x.IsDeleted == false, includes)
        {
            foreach (var filter in inFilters)
                And(filter);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySpecification"/> class with a single filter.
        /// </summary>
        /// <param name="inFilter">The filter expression.</param>
        /// <param name="includes">Include other fields to the query</param>
        public PropertySpecification(Expression<Func<Property, bool>> inFilter, List<Expression<Func<Property, object>>> includes)
            : base(x => x.IsDeleted == false, includes)
        {
            And(inFilter);
        }
    }
}
