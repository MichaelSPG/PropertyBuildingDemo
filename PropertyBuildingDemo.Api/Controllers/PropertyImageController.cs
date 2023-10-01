// <copyright file="PropertyImagesController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using PropertyBuildingDemo.Application.Dto;

namespace PropertyBuildingDemo.Api.Controllers;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PropertyBuildingDemo.Domain.Entities;
using PropertyBuildingDemo.Domain.Interfaces;

/// <summary>
/// Represents the API controller for managing property images in API version 1.0.
/// </summary>
[ApiVersion("1.0")]
public class PropertyImageController : GenericEntityController<PropertyImage, PropertyImageDto>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyImageController"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work for managing data operations.</param>
    /// <param name="systemLogger">The logger for system-related activities and events.</param>
    /// <param name="mapper">The mapper for mapping between entity and DTO objects.</param>
    public PropertyImageController(IUnitOfWork unitOfWork, ISystemLogger systemLogger, IMapper mapper)
        : base(unitOfWork, systemLogger, mapper)
    {
    }
}