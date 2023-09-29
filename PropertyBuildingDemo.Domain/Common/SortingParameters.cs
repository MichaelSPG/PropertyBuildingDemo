using PropertyBuildingDemo.Domain.Entities.Enums;

namespace PropertyBuildingDemo.Domain.Common
{
    /// <summary>
    /// Represents sorting parameters for ordering data.
    /// </summary>
    public class SortingParameters
    {
        /// <summary>
        /// Gets or sets the sorting direction (ascending or descending).
        /// </summary>
        public ESortingDirection Direction { get; set; }

        /// <summary>
        /// Gets or sets the target field to use for sorting.
        /// </summary>
        public string TargetField { get; set; }

        /// <summary>
        /// Gets or sets the priority of the sorting parameter.
        /// </summary>
        public int Priority { get; set; }
    }
}
