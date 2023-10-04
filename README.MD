# PropertyBuilding Demo Documentation

## Table of Contents

1. [Introduction](#introduction)
2. [Prerequisites](#prerequisites)
3. [Installing Redis](#installing-redis)
4. [Project Overview](#project-overview)
5. [Architecture](#architecture)
6. [Project Structure](#project-structure)
7. [Documentation and Code Comments](#documentation-and-code-comments)
8. [Best Practices](#best-practices)
9. [Performance Management](#performance-management)
10. [Integration Testing](#integration-testing)
11. [Security](#security)
12. [Development_Tools](#Development Tools)
13. [API Services](#api-services)
    a. [Create Property Building](#create-property-building)
    b. [Add Image to Property](#add-image-to-property)
    c. [Change Property Price](#change-property-price)
    d. [Update Property](#update-property)
    e. [List Properties with Filters](#list-properties-with-filters)

## Introduction

The PropertyBuilding Demo is a .NET 6-based project designed to meet the requirements of a large Real Estate company. This documentation outlines the project's architecture, structure, coding practices, testing, and security considerations.

## Prerequisites

To run the PropertyBuilding Demo project, you must have the following prerequisites:

- .NET 6 or higher installed in Visual Studio.
- SQL Server.
- C#.
- NUnit (for integration testing).
- Redis (for caching).

## Installing Redis

Redis is used for caching in the PropertyBuilding Demo project. To install Redis, follow these steps:

1. Download the latest Redis MSI installer for Windows from the [official Redis website](https://redis.io/download).

2. Run the Redis MSI installer and follow the installation instructions.

3. After the installation is complete, start the Redis server.

4. Verify that Redis is running by opening a command prompt and running the following command:

redis-cli ping


If Redis is running, it will respond with "PONG."

## Project Overview

The PropertyBuilding Demo project aims to create a robust API that allows users to interact with property information. It includes the following key features:

- Create Property Building: Add new property information to the database.
- Add Image to Property: Attach images to existing property listings.
- Change Property Price: Update the price of a property listing.
- Update Property: Modify property details and attributes.
- List Properties with Filters: Retrieve property listings based on specific criteria.

## Architecture

The project follows a clean and scalable architecture, separating concerns into layers:

- **Presentation Layer**: Contains the API controllers and handles user requests.
- **Application Layer**: Contains application services that orchestrate business logic.
- **Domain Layer**: Defines domain models and entities.
- **Persistence Layer**: Manages data access to SQL Server through Entity Framework.
- **Infrastructure Layer**: Provides cross-cutting concerns such as logging and caching.

## Project Structure

The project structure is organized as follows:

- `PropertyBuilding.Api`: API controllers and endpoints.
- `PropertyBuilding.Application`: Application services and DTOs.
- `PropertyBuilding.Domain`: Domain models and entities.
- `PropertyBuilding.Persistence`: Data access using Entity Framework.
- `PropertyBuilding.Infrastructure`: Cross-cutting concerns.

## Documentation and Code Comments

The codebase is well-documented using inline comments and XML documentation comments. This ensures that developers can understand the code's functionality, inputs, and outputs.

## Best Practices

The project adheres to best practices, including SOLID principles and design patterns to ensure maintainability and scalability.

## Performance Management

Efforts have been made to optimize database queries, and caching strategies are implemented to enhance performance.

## Integration Testing

Integration testing is performed instead of unit testing to validate the interactions between different components and ensure the system's overall functionality.

## Security

Security considerations are taken seriously. The API implements authentication and authorization mechanisms to protect sensitive data and ensure that only authorized users can access certain endpoints.

## Development_Tools

In the PropertyBuilding Demo project, we make use of various development tools to streamline the development process and ensure code quality. One such tool is ReSharper for Visual Studio. Here's how ReSharper contributes to our development environment:

## Additional Components for Clean Code

In the PropertyBuilding Demo project, we emphasize the importance of clean, maintainable code. To help achieve this goal, we use additional development tools, such as ReSharper for Visual Studio. ReSharper is a powerful extension that enhances code quality, readability, and productivity. Here's how ReSharper contributes to clean code in our project:

### Code Analysis and Inspections

ReSharper performs static code analysis and provides real-time code inspections. It highlights code issues, suggests improvements, and enforces coding standards. This helps our development team identify and address potential problems before they become critical.

### Code Refactoring

ReSharper offers a wide range of code refactoring options, making it easy to restructure and optimize code. It simplifies complex code blocks, extracts methods, renames variables, and eliminates code smells. This ensures that our codebase remains clean and maintainable over time.

### Code Navigation and Exploration

ReSharper enhances code navigation and exploration within Visual Studio. It provides intelligent code search, quick navigation to related code elements, and a powerful code explorer. This accelerates development and makes it easier to understand and modify code.

### Code Documentation

ReSharper assists in generating XML documentation comments and keeping them up to date. Proper documentation ensures that our codebase is well-documented, making it easier for developers to understand the purpose and usage of classes, methods, and properties.

### Unit Testing Support

ReSharper offers excellent support for unit testing, including test creation, execution, and result visualization. This helps us maintain a robust suite of unit tests to validate our code's correctness and reliability.

### Code Formatting and Style

ReSharper enforces code formatting and coding style rules based on industry best practices and custom configurations. Consistent code formatting enhances code readability and maintainability.

By using ReSharper alongside Visual Studio, we aim to maintain a clean and high-quality codebase throughout the development process. This tool empowers our development team to write code that is more efficient, reliable, and easier to maintain, ultimately delivering a better product to our clients.

## API Services

### Create Property Building

- **Endpoint**: `/api/property/create`
- **Description**: Adds a new property listing to the database.
- **Request**: Property details in JSON format.
- **Response**: Confirmation message or error message.

### Add Image to Property

- **Endpoint**: `/api/property/add-image/{propertyId}`
- **Description**: Attaches images to an existing property listing.
- **Request**: Image file(s) to upload.
- **Response**: Confirmation message or error message.

### Change Property Price

- **Endpoint**: `/api/property/change-price/{propertyId}`
- **Description**: Updates the price of a property listing.
- **Request**: New price value.
- **Response**: Confirmation message or error message.

### Update Property

- **Endpoint**: `/api/property/update/{propertyId}`
- **Description**: Modifies property details and attributes.
- **Request**: Updated property details in JSON format.
- **Response**: Confirmation message or error message.

### List Properties with Filters

- **Endpoint**: `/api/property/list`
- **Description**: Retrieves property listings based on specific criteria.
- **Request**: Filtering parameters in JSON format.
- **Response**: List of property listings that match the criteria.

This documentation provides an overview of the PropertyBuilding Demo project, its architecture, structure, and the services it offers. Developers can refer to this documentation for insights into the project's functionality, implementation details, Redis installation instructions, and integration testing practices.
