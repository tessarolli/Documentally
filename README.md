# Documentally Project

Documentally is a monolithic application built for scaling using .NET Core and PostgreSQL as the primary data storage. 
It follows Clean Architecture, Domain-Driven Design (DDD) and CQRS (Commands and Queries Responsibility Seggregation) principles to ensure the code is organized, maintainable, and scalable.
Its loose coupling design makes it easy to refactor into a microservices architecture with minimal changes. 
The application provides an API for users to upload and download documents with metadata such as posted date, name, description, and category, as well as manage user groups and access permissions.

## Technologies Used
- .NET Core: A free, open-source, cross-platform framework for building modern, cloud-based, internet-connected applications.
- PostgreSQL: A powerful, open-source object-relational database system.
- Dapper: A simple, lightweight ORM (Object-Relational Mapping) that makes it easy to work with relational databases in .NET applications.
- Azure Blob Cloud Storage: A cloud-based object storage solution provided by Microsoft Azure, which allows you to store and access unstructured data such as text, images, and videos.
- MediatR: A library that allows for the easy implementation of the Mediator pattern by providing a simple interface for sending and handling requests between objects, typically in a request-response fashion. This can be especially useful in large applications where there are many classes and dependencies, as it helps to simplify communication and reduce coupling between different parts of the codebase.
- Mapster: A fast, convention-based object-object mapper that allows you to easily convert objects of one type to another, with support for nested mapping and customization.

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

3. Execute the User-Secrets.cmd script located in the Documentally.API folder, for registering sensitive information on local store.

4. Start the application:

`dotnet run`

## API Documentation
The API documentation can be found in the Swagger UI, which is available at `http://localhost:5000/swagger/index.html` when the application is running. The documentation includes information about each endpoint and its parameters.

## Contributors
- Luiz Tessarolli (@tessarolli)
