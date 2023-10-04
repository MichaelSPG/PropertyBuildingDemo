// <copyright file="BaseController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace PropertyBuildingDemo.Api.Controllers;

using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Base controller for API controllers in the application.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class BaseController : Controller
{
    // No additional members or methods in this base controller.
}