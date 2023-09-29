using PropertyBuildingDemo.Domain.Entities.Enums;

namespace PropertyBuildingDemo.Domain.Common
{

    public class DefaultQueryFilterArgs : PageArgs
    {
        public DefaultQueryFilterArgs()
        {
            SortingParameters = new List<SortingParameters>();
            FilteringParameters = new List<FilteringParameters>();
            LogicalOperator = ELogicalOperator.Or;
        }

        public IEnumerable<SortingParameters>   SortingParameters { get; set; }
        public IEnumerable<FilteringParameters> FilteringParameters { get; set; }
        public ELogicalOperator LogicalOperator{ get; set; }
    }
}