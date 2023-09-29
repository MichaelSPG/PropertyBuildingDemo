using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Domain.Entities.Enums;
using PropertyBuildingDemo.Domain.Specification;

namespace PropertyBuildingDemo.Domain.Specifications
{
    public class PropertySpecification : BaseSpecifications<Property>
    {
        public PropertySpecification(long inIdOwner)
            : base(x => x.IdOwner == inIdOwner)
        {
            AddInclude(o => o.Owner);
            AddOrderByDescending(od => od.IdProperty);
        }

        public PropertySpecification(List<Expression<Func<Property, bool>>> inFilters)
            : base(x => true)
        {
            foreach (var filter in inFilters)
                And(filter);
        }
        public PropertySpecification(Expression<Func<Property, bool>> inFilter)
            : base(x => true)
        {
            And(inFilter);
        }
    }
}
