# MVC Application with .NET Core

This is an MVC application built using .NET Core. It follows a three-tier architecture
(Presentation Layer, Data Access Layer, and Business Access Layer) 
and utilizes various design patterns and libraries.

## Architecture and Design Patterns

The application is structured using the following architecture and design patterns:

- Three-tier architecture: The application is organized into Presentation Layer (PAL), Data Access Layer (DAL), and Business Access Layer (BAL).
- Unit of Work and Repository Design Pattern: Used for managing data access and database operations.
- Specification Design Pattern: Implemented to build dynamic queries.

## Controllers

The application includes the following controllers:

- Employee Controller: Manages CRUD operations for employees.
- Department Controller: Manages CRUD operations for departments.
- Account Controller: Handles user registration, login, password reset, and sign out functionalities using Identity authentication.
- Role Controller: Provides CRUD operations for managing roles and assigning roles to users.
- User Controller: Enables admin control over system users, supporting CRUD operations.

## Authentication and Authorization

- Identity Authentication: The application implements authentication using the Identity framework, allowing users to register and log in.
- RoleManager: Utilized to manage user roles and permissions.

## Additional Libraries

- AutoMapper: Used for mapping between models and view models.

## Setup and Configuration

To set up and run the application, follow these steps:

1. Clone the repository.
2. Ensure you have the required dependencies installed (e.g., .NET Core SDK).
3. Configure the database connection and any other necessary settings in the application configuration files.
4. Build and run the application.

## Contributing

Contributions to this project are welcome. If you find any issues or have suggestions for improvements, please feel free to submit a pull request.


