# PropertyBuilding Demo Documentation

Welcome to the PropertyBuilding Demo project! This project showcases the development of an API with best practices with clean architecture and more tools.

## Table of Contents
1. [Introduction](#introduction)
2. [Prerequisites](#prerequisites)
3. [Installation](#installation)
4. [Building and Running the Project](#building-and-running-the-project)
5. [Project Architecture](#project-architecture)
6. [Code Structure](#code-structure)
7. [Documentation Guidelines](#documentation-guidelines)
8. [Best Practices](#best-practices)
9. [Performance Optimization](#performance-optimization)
10. [Integration Testing](#integration-testing)
11. [Security Measures](#security-measures)
12. [Development Tools](#development-tools)
13. [Additional Components](#additional-components)
14. [API Services](#api-services)
15. [JWT Authentication](#jwt-authentication)
16. [Test User for Authentication](#test-user-for-authentication)
17. [Clean Architecture](#clean-architecture)
18. [Integration Testing with NUnit](#integration-testing-with-nunit)
19. [Performance Management](#performance-management)
20. [Conclusion](#conclusion)
21. [Appendix](#appendix)

## 1. Introduction

The PropertyBuilding Demo project is aimed at developing a comprehensive API to manage property information. This API provides services to create property buildings, add images, change prices, update properties, and list properties with various filters.

## 2. Prerequisites

Before you can run the PropertyBuilding Demo project, ensure you have the following prerequisites installed:

- .NET 6 or higher
- SQL Server
- C#
- NUnit
- Redis (for caching, optional)

### Additional Components

For maintaining code quality and adhering to coding standards, we recommend using the following development tools:

- [ReSharper](https://www.jetbrains.com/resharper/) for Visual Studio (Optional)

## 3. Installation

To install and run the PropertyBuilding Demo project, follow these steps:

1. Clone this repository to your local machine.
git clone https://github.com/MichaelSPG/PropertyBuildingDemo.git


2. Open the solution in Visual Studio or your preferred code editor.

3. Restore NuGet packages and build the solution.

4. Configure your database connection string in the `appsettings.json` file.

5. Apply database migrations to create the required tables.

dotnet ef database update

## 4. Building and Running the Project

To build and run the PropertyBuilding Demo project, follow these steps:

1. Build the project.

dotnet build


2. Run the project.


## 5. Project Architecture

The PropertyBuilding Demo project follows a well-structured architecture to ensure maintainability and scalability. It includes components such as controllers, services, repositories, models, and integration tests.

## 6. Code Structure

Our codebase adheres to clean code principles, including naming conventions, SOLID principles, and design patterns.

## 7. Documentation Guidelines

We follow thorough code documentation practices to make the codebase easily understandable for other developers.

## 8. Best Practices

The project incorporates best practices for .NET development to ensure code quality and reliability.

## 9. Performance Optimization

Performance is a key aspect, and the project includes mechanisms like caching to improve response times.

## 10. Integration Testing



## 11. Security Measures

Security is a top priority, and measures are taken to protect sensitive data and prevent common security vulnerabilities.

## 12. Development Tools

In the PropertyBuilding Demo project, we make use of various development tools to streamline the development process and ensure code quality. One such tool is ReSharper for Visual Studio.

## 13. Additional Components

ReSharper for Visual Studio is recommended for enhanced coding assistance and code analysis.

## 14. API Services

The PropertyBuilding API offers the following services:
- Create Property Building
- Add Image to Property
- Change Property Price
- Update Property Information
- List Properties with Filters

## 15. JWT Authentication

All controllers in the PropertyBuilding Demo project use JWT (JSON Web Token) authentication to secure API endpoints.

## 16. Test User for Authentication

You can authenticate with the API using the default test user credentials:

```json
{
  "username": "christopher92@stwnet.com",
  "password": "@1234#"
}

## 17. Clean Architecture, Caching, and Performance

The PropertyBuilding Demo project follows a Clean Architecture pattern, which is a software architectural style that separates concerns into distinct layers, making the codebase highly maintainable and testable. The architecture consists of the following layers:

## Domain Layer

The Domain Layer represents the core business logic of the application. It includes entities, value objects, and business rules. In this layer, you'll find:

- **Entities**: Represent core business objects like properties, owners, images, and more.
- **Value Objects**: Represent immutable objects that hold values but have no distinct identity.
- **Business Rules**: Define the constraints and logic that govern the behavior of the domain.

## Application Layer

The Application Layer contains the application-specific logic and use cases. It acts as an intermediary between the Domain Layer and the Infrastructure Layer. Key components in this layer include:

- **Use Cases**: Implement specific application functionality, such as creating property buildings, updating property information, and more.
- **DTOs (Data Transfer Objects)**: Define data structures used for communication between layers.
- **Mappers**: Transform entities to DTOs and vice versa.
- **Interfaces**: Define contracts for services that the Infrastructure Layer will implement.

### Infrastructure Layer

The Infrastructure Layer is responsible for handling external concerns such as databases, caching, and external services. This layer includes:

- **Data Access**: Implement database repositories and data access logic.
- **Persistence**: Configure database contexts and migrations.
- **Caching (Redis)**: Utilizes Redis caching mechanisms to improve application performance by reducing database queries.
- **External Services**: Interact with external services or APIs.

## API Layer

The API Layer represents the presentation layer, providing HTTP endpoints to interact with the application. It includes controllers, models, and routing. Controllers define API endpoints, while models define request and response structures.

**Performance Management**

To ensure optimal performance, the PropertyBuilding Demo project incorporates caching with Redis. Redis is used to store frequently accessed data, reducing the need for expensive database queries and enhancing overall application speed.

The Clean Architecture promotes the following benefits:

- **Testability**: Each layer can be tested independently, allowing for robust unit and integration tests.
- **Maintainability**: Clear separation of concerns makes it easier to modify or extend the application.
- **Scalability**: The architecture can accommodate changes in business logic and scaling requirements.

The PropertyBuilding Demo project demonstrates the advantages of Clean Architecture while leveraging Redis for efficient caching and improved performance.


