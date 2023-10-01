// <copyright file="PropertyTraceController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PropertyBuildingDemo.Api.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PropertyBuildingDemo.Application.Dto;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Domain.Interfaces;

/// <summary>
/// Represents the API controller for managing property traces in API version 1.0.
/// </summary>
[ApiVersion("1.0")]
public class PropertyTraceController : GenericEntityController<PropertyTrace, PropertyTraceDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyTraceController"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work for managing data operations.</param>
    /// <param name="systemLogger">The logger for system-related activities and events.</param>
    /// <param name="mapper">The mapper for mapping between entity and DTO objects.</param>
    public PropertyTraceController(IUnitOfWork unitOfWork, ISystemLogger systemLogger, IMapper mapper)
        : base(unitOfWork, systemLogger, mapper)
    {
    }
}