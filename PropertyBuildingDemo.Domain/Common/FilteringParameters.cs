using PropertyBuildingDemo.Domain.Entities.Enums;

namespace PropertyBuildingDemo.Domain.Common
{
    /// <summary>
    /// Represents filtering parameters used for filtering data.
    /// </summary>
    public class FilteringParameters
    {
        /// <summary>
        /// Gets or sets the target field to filter by.
        /// </summary>
        public string TargetField { get; set; }

        /// <summary>
        /// Gets or sets the comparison operator for filtering.
        /// </summary>
        public EComparisionOperator ComparisionOperator { get; set; }

        /// <summary>
        /// Gets or sets the value to compare when filtering.
        /// </summary>
        public string Value { get; set; }
    }
}