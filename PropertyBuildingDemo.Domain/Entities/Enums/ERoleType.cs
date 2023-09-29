using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyBuildingDemo.Domain.Entities.Enums
{
    public enum ERoleType
    {
        [Description("Basic role")]
        RoleBasic,
        [Description("Customer role")]
        RoleCustomer,
        [Description("Seller role")]
        RoleSeller,
        [Description("Property owner role")]
        RolePropertyOwner,
        [Description("Admin role")]
        RoleAdmin,

    }
}
