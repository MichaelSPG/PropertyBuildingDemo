PropertyBuilding Demo Documentation
Table of Contents
Introduction
Prerequisites
Installing Redis
Project Overview
Architecture
Project Structure
Documentation and Code Comments
Best Practices
Performance Management
Integration Testing
Security
API Services
a. Create Property Building
b. Add Image to Property
c. Change Property Price
d. Update Property
e. List Properties with Filters
Introduction
The PropertyBuilding Demo is a .NET 6-based project designed to meet the requirements of a large Real Estate company. It provides a set of API services for managing property information in the United States. This documentation outlines the project's architecture, structure, coding practices, testing, and security considerations.

Prerequisites
To run the PropertyBuilding Demo project, you must have the following prerequisites:

.NET 6 or higher installed in Visual Studio.
SQL Server.
C#.
NUnit (for integration testing).
Redis (for caching).
Installing Redis
Redis is used for caching in the PropertyBuilding Demo project. To install Redis, follow these steps:

Download the latest Redis MSI installer for Windows from the official Redis website.

Run the Redis MSI installer and follow the installation instructions.

After the installation is complete, start the Redis server.

Verify that Redis is running by opening a command prompt and running the following command:

Copy code
redis-cli ping
If Redis is running, it will respond with "PONG."

Project Overview
The PropertyBuilding Demo project aims to create a robust API that allows users to interact with property information. It includes the following key features:

Create Property Building: Add new property information to the database.
Add Image to Property: Attach images to existing property listings.
Change Property Price: Update the price of a property listing.
Update Property: Modify property details and attributes.
List Properties with Filters: Retrieve property listings based on specific criteria.
Architecture
The project follows a clean and scalable architecture, separating concerns into layers:

Presentation Layer: Contains the API controllers and handles user requests.
Application Layer: Contains application services that orchestrate business logic.
Domain Layer: Defines domain models and entities.
Persistence Layer: Manages data access to SQL Server through Entity Framework.
Infrastructure Layer: Provides cross-cutting concerns such as logging and caching.
Project Structure
The project structure is organized as follows:

PropertyBuilding.Api: API controllers and endpoints.
PropertyBuilding.Application: Application services and DTOs.
PropertyBuilding.Domain: Domain models and entities.
PropertyBuilding.Persistence: Data access using Entity Framework.
PropertyBuilding.Infrastructure: Cross-cutting concerns.
Documentation and Code Comments
The codebase is well-documented using inline comments and XML documentation comments. This ensures that developers can understand the code's functionality, inputs, and outputs.

Best Practices
The project adheres to best practices, including SOLID principles and design patterns to ensure maintainability and scalability.

Performance Management
Efforts have been made to optimize database queries, and caching strategies are implemented to enhance performance.

Integration Testing
Integration testing is performed instead of unit testing to validate the interactions between different components and ensure the system's overall functionality.

Security
Security considerations are taken seriously. The API implements authentication and authorization mechanisms to protect sensitive data and ensure that only authorized users can access certain endpoints.

API Services
Create Property Building
Endpoint: /api/property/create
Description: Adds a new property listing to the database.
Request: Property details in JSON format.
Response: Confirmation message or error message.
Add Image to Property
Endpoint: /api/property/add-image/{propertyId}
Description: Attaches images to an existing property listing.
Request: Image file(s) to upload.
Response: Confirmation message or error message.
Change Property Price
Endpoint: /api/property/change-price/{propertyId}
Description: Updates the price of a property listing.
Request: New price value.
Response: Confirmation message or error message.
Update Property
Endpoint: /api/property/update/{propertyId}
Description: Modifies property details and attributes.
Request: Updated property details in JSON format.
Response: Confirmation message or error message.
List Properties with Filters
Endpoint: /api/property/list
Description: Retrieves property listings based on specific criteria.
Request: Filtering parameters in JSON format.
Response: List of property listings that match the criteria.
This documentation provides an overview of the PropertyBuilding Demo project, its architecture, structure, and the services it offers. Developers can refer to this documentation for insights into the project's functionality, implementation details, Redis installation instructions, and integration testing practices.

https://github.com/microsoftarchive/redis/releases/tag/win-3.0.504