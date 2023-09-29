using PropertyBuildingDemo.Domain.Entities.Enums;

namespace PropertyBuildingDemo.Domain.Common
{
    public class FilteringParameters
    {
        public string TargetField { get; set; }
        public EComparisionOperator ComparisionOperator { get; set; }
        public string Value { get; set; }
    }
}