// <copyright file="OwnerController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoMapper;
using PropertyBuildingDemo.Application.Dto;

namespace PropertyBuildingDemo.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Domain.Interfaces;

/// <summary>
/// Controller responsible for managing operations related to owners of properties.
/// </summary>
[ApiVersion("1.0")]
public class OwnerController : GenericEntityController<Owner, OwnerDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OwnerController"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work for database operations.</param>
    /// <param name="systemLogger">The system logger for logging exceptions.</param>
    /// <param name="mapper">The system mapper for mapping the DTOs.</param>
    public OwnerController(IUnitOfWork unitOfWork, ISystemLogger systemLogger, IMapper mapper)
        : base(unitOfWork, systemLogger, mapper)
    {
        // The constructor initializes the base class with the provided unit of work and system logger.
    }

    // Additional owner-specific actions can be added here as needed.
}