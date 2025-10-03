# Disaster Alleviation Foundation Web Application

This is an ASP.NET Core 8.0 MVC web application for managing disaster relief efforts, including incident reporting, donation tracking, and volunteer management.

## Prerequisites

Before running this application, ensure you have the following installed:

1. **.NET 8.0 SDK** - Download from [https://dotnet.microsoft.com/download/dotnet/8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
2. **SQL Server LocalDB** (for development) or **Azure SQL Database** (for production)
3. **Visual Studio Code** with C# extension
4. **Git** for version control

## Quick Start

### 1. Install .NET 8.0 SDK
Download and install the .NET 8.0 SDK from the official Microsoft website: https://dotnet.microsoft.com/download/dotnet/8.0

### 2. Verify Installation
Open a terminal and run:
```bash
dotnet --version
```
This should return version 8.0.x

### 3. Clone and Setup
```bash
# Navigate to the project directory
cd DisasterAlleviationFoundation

# Restore NuGet packages
dotnet restore

# Install Entity Framework tools (if not already installed)
dotnet tool install --global dotnet-ef
```

### 4. Database Setup
```bash
# Create initial migration (if migrations folder doesn't exist)
dotnet ef migrations add InitialCreate

# Update database with migrations
dotnet ef database update
```

### 5. Run the Application
```bash
# Run in development mode
dotnet run

# Or run with hot reload
dotnet watch run
```

The application will be available at:
- HTTPS: `https://localhost:7001`
- HTTP: `http://localhost:5000`

### 6. Login with Default Admin Account
- **Email**: admin@disasteralleviation.org
- **Password**: Admin123!

## Features

### 🔐 Authentication & Authorization
- User registration and login with ASP.NET Core Identity
- Role-based access control (Admin, Volunteer, Donor, Regular User)
- Secure password requirements and validation

### 📋 Incident Reporting
- Report disaster incidents with location and description
- Track incident status and priority levels
- Admin approval and management workflow

### 🎁 Donation Management
- Register resource donations (food, clothing, medical supplies, etc.)
- Track donation status from pending to distributed
- Donor contact information and delivery coordination

### 👥 Volunteer Management
- Volunteer task creation and assignment
- Skill-based task matching
- Task completion tracking and reporting

### 📊 Dashboard & Analytics
- Overview of all activities for different user roles
- Recent incidents, donations, and volunteer tasks
- Status summaries and key metrics

## Database Setup

### Development (LocalDB)
The application uses SQL Server LocalDB for development. The connection string is configured in `appsettings.json`.

### Production (Azure SQL Database)
For production deployment:

1. Create an Azure SQL Database
2. Update the connection string in `appsettings.Production.json`
3. Run migrations: `dotnet ef database update`

## Project Structure

```
DisasterAlleviationFoundation/
├── Controllers/          # MVC Controllers
├── Data/                # Entity Framework DbContext
├── Models/              # Data models and ViewModels
├── Services/            # Business logic services
├── Views/               # Razor views
├── wwwroot/             # Static files (CSS, JS, images)
├── Program.cs           # Application entry point
└── appsettings.json     # Configuration
```

## Default Users

The application creates a default admin user:
- **Email**: admin@disasteralleviation.org
- **Password**: Admin123!

## Entity Framework Migrations

### Create a new migration:
```bash
dotnet ef migrations add MigrationName
```

### Update database:
```bash
dotnet ef database update
```

### Remove last migration:
```bash
dotnet ef migrations remove
```

## Development Workflow

1. **Models**: Define data models in `Models/` folder
2. **Controllers**: Create controllers for handling HTTP requests
3. **Views**: Build Razor views for user interface
4. **Services**: Implement business logic in service classes
5. **Migrations**: Update database schema as needed

## Security Features

- Password strength requirements
- Role-based authorization
- HTTPS enforcement
- Input validation and sanitization
- CSRF protection
- SQL injection prevention through Entity Framework

## Deployment

### Azure App Service Deployment

1. **Prepare for deployment**:
   ```bash
   dotnet publish -c Release -o ./publish
   ```

2. **Azure CLI deployment**:
   ```bash
   az webapp deployment source config-zip --resource-group myResourceGroup --name myAppName --src publish.zip
   ```

3. **Set connection string** in Azure App Service configuration.

### Docker Deployment

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY publish/ .
ENTRYPOINT ["dotnet", "DisasterAlleviationFoundation.dll"]
```

## Contributing

1. Fork the repository
2. Create a feature branch: `git checkout -b feature-name`
3. Commit your changes: `git commit -am 'Add feature'`
4. Push to the branch: `git push origin feature-name`
5. Submit a pull request

## Support

For technical support or questions about this application, please contact the development team or create an issue in the repository.

## License

This project is licensed under the MIT License - see the LICENSE file for details.