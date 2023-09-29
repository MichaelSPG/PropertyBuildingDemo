﻿// <copyright file="OwnerController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PropertyBuildingDemo.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Domain.Interfaces;

/// <summary>
/// Controller responsible for managing operations related to owners of properties.
/// </summary>
[ApiVersion("1.0")]
public class OwnerController : GenericEntityController<Owner>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OwnerController"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work for database operations.</param>
    /// <param name="systemLogger">The system logger for logging exceptions.</param>
    public OwnerController(IUnitOfWork unitOfWork, ISystemLogger systemLogger)
        : base(unitOfWork, systemLogger)
    {
        // The constructor initializes the base class with the provided unit of work and system logger.
    }

    // Additional owner-specific actions to be added here.
}