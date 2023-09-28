using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyBuildingDemo.Domain.Common;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Domain.Interfaces;
using PropertyBuildingDemo.Infrastructure.Data;
using PropertyBuildingDemo.Domain.Entities.Enums;
using TEntityBuildingDemo.Api.Controllers;

namespace PropertyBuildingDemo.Api.Controllers
{

    [ApiVersion("1.0")]
    public class PropertyTraceController : GenericEntityController<PropertyTrace>
    {
        public PropertyTraceController(IUnitOfWork unitOfWork, ISystemLogger systemLogger) : base(unitOfWork, systemLogger)
        {
        }
    }
}
