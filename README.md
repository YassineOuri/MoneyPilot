# MoneyPilot API

A personal finance management REST API built with ASP.NET Core that helps users track their accounts and transactions.

## Features

- 🔐 **User Authentication** - JWT-based authentication with secure password hashing
- 💰 **Account Management** - Create, read, update, and delete financial accounts
- 📊 **Transaction Tracking** - Record income and expense transactions
- 🔒 **Authorization** - Users can only access their own accounts and transactions
- 📝 **Swagger Documentation** - Interactive API documentation with Swagger UI

## Tech Stack

- **.NET 10.0** - Framework
- **ASP.NET Core** - Web API framework
- **Entity Framework Core 9.0** - ORM
- **MySQL** - Database (via Pomelo.EntityFrameworkCore.MySql)
- **JWT Bearer Authentication** - Token-based authentication
- **BCrypt.Net** - Password hashing
- **Swashbuckle/Swagger** - API documentation

## Prerequisites

- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- MySQL Server (or MySQL-compatible database)
- A `.env` file with required environment variables (see below)

## Setup Instructions

### 1. Clone the Repository

```bash
git clone <repository-url>
cd MoneyPilot
```

### 2. Install Dependencies

```bash
dotnet restore
```

### 3. Configure Environment Variables

Create a `.env` file in the root directory with the following variables:

```env
JWT_SECRET=your-secret-key-here-minimum-32-characters
DB_CONNECTION_STRING=Server=localhost;Database=MoneyPilotDB;User=your_user;Password=your_password;
```

**Important:** 
- `JWT_SECRET` should be a strong, random string (at least 32 characters)
- `DB_CONNECTION_STRING` should match your MySQL database configuration

### 4. Database Setup

Run Entity Framework migrations to create the database schema:

```bash
dotnet ef database update
```

If you need to create a new migration:

```bash
dotnet ef migrations add MigrationName
```

### 5. Run the Application

```bash
dotnet run
```

The API will be available at:
- **HTTP:** `http://localhost:5000`
- **HTTPS:** `https://localhost:5001`
- **Swagger UI:** `https://localhost:5001/swagger` (in Development mode)

## API Endpoints

### Authentication (`/api/auth`)

- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Login and receive JWT token

### Accounts (`/api/accounts`)

- `GET /api/accounts` - Get all accounts for authenticated user
- `POST /api/accounts` - Create a new account
- `PUT /api/accounts/{id}` - Update an account
- `DELETE /api/accounts/{id}` - Delete an account

### Transactions (`/api/transactions`)

- `GET /api/transactions/user/{userId}` - Get all transactions for a user
- `GET /api/transactions/account/{accountId}` - Get all transactions for an account
- `POST /api/transactions` - Create a new transaction
- `PUT /api/transactions/{id}` - Update a transaction
- `DELETE /api/transactions/{id}` - Delete a transaction

**Note:** Most endpoints require JWT authentication. Include the token in the Authorization header:
```
Authorization: Bearer {your-jwt-token}
```

## Project Structure

```
MoneyPilot/
├── Controllers/          # API controllers
│   ├── AccountController.cs
│   ├── AuthController.cs
│   └── TransactionController.cs
├── Data/                 # Database context
│   └── ApplicationDbContext.cs
├── DTO/                  # Data Transfer Objects
│   ├── AccountDTO.cs
│   ├── Transactions/
│   └── UserLoginDTO.cs
├── Models/               # Entity models
│   ├── Account.cs
│   ├── Transaction.cs
│   └── User.cs
├── Services/             # Business logic services
│   ├── AuthService.cs
│   ├── PasswordService.cs
│   └── TransactionService.cs
├── Migrations/           # Entity Framework migrations
└── Program.cs            # Application entry point
```

## Development

### Running in Development Mode

The application runs in Development mode by default, which includes:
- Swagger UI for API testing
- Detailed error messages
- Console logging

### Building the Project

```bash
dotnet build
```

## Security Features

- Passwords are hashed using BCrypt
- JWT tokens expire after 30 minutes
- Users can only access their own resources
- Input validation on all endpoints
- SQL injection protection via Entity Framework



