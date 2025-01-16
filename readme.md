# Library Management API

A .NET Core Web API for managing a library system with book and user management capabilities.

## Features

- User authentication using JWT tokens
- Book management (CRUD operations)
- User management
- Dashboard statistics
- SQLite database integration

## Technologies

- .NET 8.0
- Entity Framework Core 9.0
- JWT Authentication
- SQLite Database
- Swagger/OpenAPI

## Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or VS Code

## Getting Started

1. Clone the repository
2. Navigate to the project directory
3. Run the following commands:

```sh
dotnet restore
dotnet ef database update
dotnet run
```

The API will be available at `http://localhost:5127`

## API Endpoints

### Authentication
- POST `/api/Auth/signup` - Register new user
- POST `/api/Auth/signin` - Login user
- GET `/api/Auth/verify` - Verify JWT token

### Books
- GET `/api/Books` - Get all books
- GET `/api/Books/{id}` - Get book by ID
- GET `/api/Books/recent` - Get recently added books
- POST `/api/Books` - Add new book
- PUT `/api/Books/{id}` - Update book
- DELETE `/api/Books/{id}` - Delete book

### Dashboard
- GET `/api/Dashboard` - Get system statistics


## Database Configuration

Database connection string can be configured in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DBConnectionString": "Data Source=library.db"
  }
}
```

## Authentication

The API uses JWT Bearer token authentication. Include the token in the Authorization header:

```
Authorization: Bearer <your-token>
```

## Development

To run the project in development mode with hot reload:

```sh
dotnet watch run
```

Access Swagger UI at: `http://localhost:5127/swagger`