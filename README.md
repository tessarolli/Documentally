# Documentally Project

Documentally is a monolithic application built for scaling using .NET Core and PostgreSQL as the primary data storage. 
It follows Clean Architecture, Domain-Driven Design (DDD) and CQRS (Commands and Queries Responsibility Seggregation) principles to ensure the code is organized, maintainable, and scalable. 
The application provides an API for users to upload and download documents with metadata such as posted date, name, description, and category, as well as manage user groups and access permissions.

## Technologies Used
- .NET Core
- PostgreSQL
- Dapper
- Azure Blob Cloud Storage
- MediatR
- Mapster

## Features
- User authentication
- User management (CRUD)
- Group management (CRUD)
- Document upload and download
- Group and user access permissions for documents
- Role-based access control (regular, manager, and admin)
- REST API for all actions

## Architecture
The application follows Clean Architecture and Domain-Driven Design (DDD) principles to separate the application into distinct layers that can be independently tested and developed. 
The architecture consists of the following layers:

### Presentation Layer
This layer consists of the REST API, which is responsible for handling requests and returning responses to clients.

### Application Layer
This layer contains the application logic, which is responsible for coordinating actions between the Presentation and Domain layers. 
It also implements the role-based access control (RBAC) and enforces user permissions, and defines the interfaces for the application services and repositories, which are implemented in the Infrastructure layer.

### Domain Layer
This layer contains the business logic and domain objects, including entities, aggregates, and value objects. 

### Infrastructure Layer
This layer provides concrete implementations of the application services and repositories defined in the Domain layer. 
It is also responsible for handling data access and database communication using Dapper and PostgreSQL.

## Installation and Setup
To run the application locally, follow these steps:

1. Clone the repository:

`git clone https://github.com/tessarolli/Documentally.git`

2. Install dependencies:

`dotnet restore`

3. Start the application:

`dotnet run`

4. Execute the User-Secrets.cmd script located in the Documentally.API folder, for registering sensitive information on local store.

## API Documentation
The API documentation can be found in the Swagger UI, which is available at `http://localhost:5000/swagger/index.html` when the application is running. The documentation includes information about each endpoint and its parameters.

## Contributors
- Luiz Tessarolli (@tessarolli)
