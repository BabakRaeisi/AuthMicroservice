# Auth Microservice (.NET 9, PostgreSQL, Docker)

A production-style **Authentication microservice** built with **ASP.NET Core Web API**, **Dapper**, **PostgreSQL**, and **JWT**.  
This service handles user **registration** and **login**, stores users in PostgreSQL, and issues JWT access tokens for authenticated access.

---

## Resume Summary

Designed and implemented a containerized authentication microservice using .NET 9 and PostgreSQL with secure password hashing (BCrypt), JWT-based authentication, centralized exception middleware, and Docker Compose orchestration for API + database.

---

## Key Features

- User registration endpoint
- User login endpoint
- JWT token generation
- Password hashing with BCrypt
- PostgreSQL persistence with Dapper
- Layered architecture (`API`, `Core`, `Infrastructure`)
- Swagger/OpenAPI support
- Global exception handling middleware
- Dockerized deployment with Docker Compose

---

## Tech Stack

- **Backend:** ASP.NET Core 9 Web API
- **Language:** C#
- **Data Access:** Dapper
- **Database:** PostgreSQL 17
- **Auth:** JWT Bearer Tokens
- **Password Security:** BCrypt.Net
- **Containerization:** Docker, Docker Compose
- **Validation:** FluentValidation

---

## Project Structure

```text
Auth.API/              --> API layer (controllers, middleware, config)
Auth.Core/             --> business logic, DTOs, contracts, entities
Auth.Infrastructure/   --> repositories, db context, token service, SQL scripts
docker-compose.yml     --> API + PostgreSQL orchestration
```

---

## Architecture Overview

### 1) `Auth.API`

- Exposes REST endpoints (`/api/Auth/register`, `/api/Auth/login`)
- Configures JWT authentication
- Adds middleware and DI registrations
- Hosts Swagger UI

### 2) `Auth.Core`

- Contains DTOs (`RegisterRequest`, `LoginRequest`, `AuthenticationResponse`)
- Defines contracts (`IUserService`, `IUserRepository`, `ITokenService`)
- Implements business/service logic (`UserService`)
- Defines entities (`ApplicationUser`, `JWTSetting`)

### 3) `Auth.Infrastructure`

- Implements repository with Dapper (`UserRepository`)
- Manages DB connections (`DapperDbContext`)
- Generates JWT tokens (`TokenService`)
- Stores DB initialization scripts (`Scripts/init.sql`)

---

## API Endpoints

### Register

- **POST** `/api/Auth/register`
- Creates a new user and returns JWT token.

### Login

- **POST** `/api/Auth/login`
- Validates credentials and returns JWT token.

### Example Register Request

```json
{
  "email": "user@example.com",
  "password": "StrongPassword123!",
  "personName": "User Name",
  "gender": "Male"
}
```

### Example Success Response

```json
{
  "userID": "guid-here",
  "email": "user@example.com",
  "personName": "User Name",
  "token": "jwt-token-here",
  "isSuccessful": true
}
```

---

## Security Implementation

- Passwords are hashed with **BCrypt** before storage.
- Password hashes are verified during login.
- JWT includes standard claims (subject, email, name, jti, expiration, issuer, audience).
- Sensitive settings are configurable via appsettings/environment variables.
- Password is **not** returned in API response.

---

## Local Development Setup

### Prerequisites

- .NET 9 SDK
- PostgreSQL
- (Optional) pgAdmin

### Run locally

1. Create database: `AuthUsers`
2. Run SQL script from `Auth.Infrastructure/Scripts/init.sql`
3. Update `Auth.API/appsettings.Development.json` connection string if needed
4. Run:
   ```powershell
   dotnet run --project .\Auth.API\Auth.API.csproj
   ```
5. Open Swagger:
   - `https://localhost:7001/swagger`
   - or `http://localhost:5001/swagger`

---

## Docker Setup

### Run with Docker Compose

```powershell
docker compose up --build
```

### Access

- API: `http://localhost:5001`
- Swagger: `http://localhost:5001/swagger`
- PostgreSQL: `localhost:5432`

### Reset DB volume (if schema/init changes)

```powershell
docker compose down -v
docker compose up --build
```

---

## Database Schema (Users)

The service uses a `public."Users"` table with:

- `UserID` (UUID, PK)
- `Email` (unique)
- `PersonName`
- `Gender`
- `Password` (BCrypt hash)

---

## Notable Engineering Decisions

- Used **Dapper** for lightweight, high-control SQL access.
- Kept clear separation of concerns with layered architecture.
- Added centralized error middleware for consistent error responses.
- Containerized API and DB for consistent local/dev deployment.

---

## Future Improvements

- Add refresh tokens and token rotation
- Add email verification and password reset flow
- Add rate limiting and brute-force protection
- Add migration tooling/versioned schema
- Add unit/integration tests and CI pipeline
- Move secrets to `.env`/secret manager for production

---

## Author

**Babak**  
Authentication Microservice project for portfolio/resume demonstration.
