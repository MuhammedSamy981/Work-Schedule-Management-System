# Work Schedule Management System

A backend **Work Schedule Management System** built with **ASP.NET Core 8 Web API**, **C#**, **Dapper**, and **SQL Server**.

The system provides RESTful APIs for managing employees, recording attendance and departure times, and generating monthly employee reports including lateness, early departures, and salary deductions.

## 🚀 Technologies

* **.NET 8**
* **ASP.NET Core Web API**
* **C#**
* **Dapper**
* **Microsoft SQL Server**
* **Microsoft.Data.SqlClient**
* **Swagger / OpenAPI**
* **Repository Pattern**
* **Dependency Injection**
* **Stored Procedures**
* **SQL Views**
* **Async/Await**
* **Pagination**

## 📁 Project Structure

```text
Work Schedule Management System/
│
├── API/
│   ├── Controllers/
│   │   ├── EmployeesController.cs
│   │   └── AttendanceAndDepartureTimesController.cs
│   │
│   ├── Database/
│   │   ├── ConnectionFactory/
│   │   │   ├── IDbConnectionFactory.cs
│   │   │   └── SqlConnectionFactory.cs
│   │   │
│   │   └── Models/
│   │       ├── Employee.cs
│   │       ├── AddEmployee.cs
│   │       ├── UpdateEmployee.cs
│   │       ├── AttendanceAndDepartureTime.cs
│   │       ├── AddAttendanceTime.cs
│   │       ├── AddDepartureTime.cs
│   │       └── MonthlyEmployeeReport.cs
│   │
│   ├── Repositories/
│   │   ├── Employee/
│   │   │   ├── EmployeeRepository.cs
│   │   │   └── IEmployeeRepository.cs
│   │   │
│   │   └── IAttendanceAndDepartureTime/
│   │       ├── AttendanceAndDepartureTimeRepository.cs
│   │       └── IAttendanceAndDepartureTimeRepository.cs
│   │
│   ├── Program.cs
│   ├── appsettings.json
│   └── API.csproj
│
└── DB/
    └── SQL Commands.sql
```

## ✨ Features

### Employee Management

* Get all employees
* Get employee by ID
* Add a new employee
* Update employee information
* Pagination for employee lists

### Attendance & Departure Management

* Record employee attendance time
* Record employee departure time
* Retrieve attendance/departure records
* Filter attendance records by:

  * Employee
  * Month
  * Year
* Pagination for attendance records

### Monthly Employee Reports

The system provides monthly reports containing:

* Employee ID
* Employee name
* Total late attendance minutes
* Total early departure minutes
* Salary after deductions
* Month
* Year

### Database Features

* SQL Server relational database
* Foreign key relationship between employees and attendance records
* Stored procedures for insert/update operations
* SQL View for monthly employee reports
* Parameterized SQL queries through Dapper
* Pagination using `OFFSET` and `FETCH`

## 🏗️ Architecture

The project follows a layered backend structure:

```text
HTTP Request
     │
     ▼
Controllers
     │
     ▼
Repository Interfaces
     │
     ▼
Repository Implementations
     │
     ▼
Dapper
     │
     ▼
SQL Server
```

### Controllers

Controllers expose the REST API endpoints and handle HTTP requests/responses.

Examples:

* `EmployeesController`
* `AttendanceAndDepartureTimesController`

### Repository Layer

Repositories encapsulate database access logic.

Examples:

* `EmployeeRepository`
* `AttendanceAndDepartureTimeRepository`

The repositories use interfaces to provide abstraction and support dependency injection.

### Database Connection Factory

The project uses an abstraction around database connections:

```csharp
IDbConnectionFactory
```

with the SQL Server implementation:

```csharp
SqlConnectionFactory
```

This keeps connection creation separate from repository logic.

### Dapper

Dapper is used as the micro-ORM for executing SQL queries and stored procedures and mapping database results to C# models.

Example:

```csharp
return await connection.QueryAsync<Employee>(
    sql,
    new
    {
        PageNumber = pageNumber,
        PageSize = pageSize
    });
```

## 🗄️ Database Schema

The system contains two main tables.

### Employees

```text
Employees
├── Id
├── FirstName
├── LastName
├── Email
├── Phone
├── HireDate
└── Salary
```

### AttendanceAndDepartureTimes

```text
AttendanceAndDepartureTimes
├── Id
├── AttendanceTime
├── DepartureTime
└── EmployeeId
```

Relationship:

```text
Employees
    │
    │ 1
    │
    │
    │ *
AttendanceAndDepartureTimes
```

Each employee can have multiple attendance/departure records.

## 📊 Monthly Reports

The database contains a `MonthlyEmployeeReports` SQL view.

It calculates:

```text
Total Late Attendance Minutes
+
Total Early Departure Minutes
↓
Salary Deduction
↓
Total Salary After Deductions
```

The report can be retrieved for all employees or for a specific employee/month/year.

## 🔌 API Endpoints

### Employees

#### Get Employees

```http
GET /api/Employees?pageNumber=1&pageSize=10
```

#### Get Employee By ID

```http
GET /api/Employees/{id}
```

#### Add Employee

```http
POST /api/Employees
```

Example request:

```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "phone": "555-010-1001",
  "salary": 65000
}
```

#### Update Employee

```http
PUT /api/Employees/{id}
```

### Monthly Reports

#### Get Monthly Reports

```http
GET /api/Employees/report?pageNumber=1&pageSize=10
```

#### Get Employee Monthly Report

```http
GET /api/Employees/report/{employeeId}/{month}/{year}
```

Example:

```http
GET /api/Employees/report/1/8/2026
```

### Attendance & Departure

#### Get Employee Attendance Records

```http
GET /api/AttendanceAndDepartureTimes/{employeeId}/{month}/{year}?pageNumber=1&pageSize=10
```

Example:

```http
GET /api/AttendanceAndDepartureTimes/1/8/2026?pageNumber=1&pageSize=10
```

#### Get Attendance Record

```http
GET /api/AttendanceAndDepartureTimes/{id}
```

#### Add Attendance Time

```http
POST /api/AttendanceAndDepartureTimes
```

Example:

```json
{
  "attendanceTime": "2026-08-26T08:02:00",
  "employeeId": 1
}
```

#### Add Departure Time

```http
PUT /api/AttendanceAndDepartureTimes
```

Example:

```json
{
  "id": 1,
  "departureTime": "2026-08-26T14:00:00",
  "employeeId": 1
}
```

## ⚙️ Getting Started

### Prerequisites

Make sure you have installed:

* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* SQL Server
* SQL Server Management Studio (SSMS) or another SQL Server client
* Visual Studio / Visual Studio Code

### 1. Clone the Repository

```bash
git clone https://github.com/YOUR_USERNAME/YOUR_REPOSITORY.git
```

Navigate to the project:

```bash
cd "Work Schedule Management System"
```

### 2. Create the Database

Open:

```text
DB/SQL Commands.sql
```

Execute the script in SQL Server Management Studio.

The script creates:

* `WorkScheduleManagementSystemDb`
* `Employees`
* `AttendanceAndDepartureTimes`
* Stored procedures
* `MonthlyEmployeeReports` view
* Sample data

### 3. Configure the Connection String

Open:

```text
API/appsettings.json
```

The default connection string is configured for SQL Server Express:

```json
{
  "ConnectionStrings": {
    "Default": "Data Source=.\\SQLEXPRESS;Initial Catalog=WorkScheduleManagementSystemDb;Integrated Security=true;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

Update it if your SQL Server instance uses a different server name or authentication method.

For example:

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=WorkScheduleManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

> For production environments, avoid committing sensitive connection strings directly to source control. Use environment variables, user secrets, or a secure configuration provider.

### 4. Restore Dependencies

From the API directory:

```bash
cd API
dotnet restore
```

### 5. Build the Project

```bash
dotnet build
```

### 6. Run the API

```bash
dotnet run
```

The API will start on the configured local HTTP/HTTPS ports.

### 7. Open Swagger

When running in the Development environment, Swagger/OpenAPI is enabled.

Open the Swagger URL shown in the terminal, for example:

```text
https://localhost:<port>/swagger
```

Swagger can be used to test all available endpoints.

## 🔄 Dependency Injection

The application registers the database connection factory and repositories in `Program.cs`:

```csharp
builder.Services.AddSingleton<IDbConnectionFactory, SqlConnectionFactory>();

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddScoped<
    IAttendanceAndDepartureTimeRepository,
    AttendanceAndDepartureTimeRepository
>();
```

This provides loose coupling between controllers and repository implementations.

## 📄 Stored Procedures

The database script contains stored procedures for:

```text
AddEmployee
UpdateEmployee
AddAttendanceTime
AddDepartureTime
```

These procedures are executed through Dapper.

Example:

```csharp
await connection.ExecuteAsync(
    "AddEmployee",
    employee,
    commandType: CommandType.StoredProcedure
);
```

## 📌 Pagination

The API supports pagination for employee, attendance, and monthly report queries.

Example:

```http
GET /api/Employees?pageNumber=2&pageSize=10
```

The SQL query uses:

```sql
OFFSET (@PageNumber - 1) * @PageSize ROWS
FETCH NEXT @PageSize ROWS ONLY
```

This allows the API to retrieve only the required records instead of loading the entire dataset.

## 🧪 Sample Workflow

A typical workflow is:

```text
1. Create an employee
        ↓
2. Record attendance
        ↓
3. Record departure
        ↓
4. Repeat attendance/departure records
        ↓
5. Generate monthly report
        ↓
6. Calculate lateness and early departure
        ↓
7. Calculate salary after deductions
```

## 🎯 Project Goals

This project was developed to practice and demonstrate:

* Building RESTful APIs with ASP.NET Core
* Working with SQL Server
* Using Dapper for data access
* Repository Pattern
* Dependency Injection
* Database connection abstraction
* Stored procedures
* SQL Views
* Parameterized SQL queries
* Pagination
* Asynchronous programming
* Relational database design
* Employee attendance management
* Monthly reporting

## 👨‍💻 Author

**Muhammad Samy Saber**

Backend-focused Full Stack Developer

### Skills Demonstrated

* C#
* ASP.NET Core 8
* Web API
* SQL Server
* Dapper
* REST APIs
* Repository Pattern
* Dependency Injection
* ADO.NET / `IDbConnection`
* SQL Stored Procedures
* SQL Views
* LINQ / C#
* Asynchronous Programming

## 📜 License

This project is available for educational and portfolio purposes.
