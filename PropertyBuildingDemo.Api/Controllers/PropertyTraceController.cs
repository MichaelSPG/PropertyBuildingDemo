// <copyright file="PropertyTraceController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PropertyBuildingDemo.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Domain.Interfaces;

/// <summary>
/// Represents the API controller for managing property traces in API version 1.0.
/// </summary>
[ApiVersion("1.0")]
public class PropertyTraceController : GenericEntityController<PropertyTrace>
{
    public PropertyTraceController(IUnitOfWork unitOfWork, ISystemLogger systemLogger)
        : base(unitOfWork, systemLogger)
    {
    }

    // Include your controller actions here
}