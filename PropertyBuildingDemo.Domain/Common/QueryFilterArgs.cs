using PropertyBuildingDemo.Domain.Entities.Enums;

namespace PropertyBuildingDemo.Domain.Common
{
    /// <summary>
    /// Represents default query filter parameters, including paging, sorting, and filtering options.
    /// </summary>
    public class DefaultQueryFilterArgs : PageArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultQueryFilterArgs"/> class with default values.
        /// </summary>
        public DefaultQueryFilterArgs()
        {
            SortingParameters = new List<SortingParameters>();
            FilteringParameters = new List<FilteringParameters>();
            LogicalOperator = ELogicalOperator.Or;
        }

        /// <summary>
        /// Gets or sets the list of sorting parameters for ordering the data.
        /// </summary>
        public IEnumerable<SortingParameters> SortingParameters { get; set; }

        /// <summary>
        /// Gets or sets the list of filtering parameters for filtering the data.
        /// </summary>
        public IEnumerable<FilteringParameters> FilteringParameters { get; set; }

        /// <summary>
        /// Gets or sets the logical operator to use when combining multiple filters.
        /// </summary>
        public ELogicalOperator LogicalOperator { get; set; }
    }
}