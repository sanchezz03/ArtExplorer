## ArtExplorer Project

### Overview
### ArtExplorer is a .NET-based application structured into three main layers:
- ArtExplorer.API – Web API for handling HTTP requests.
- ArtExplorer.BLL – Business Logic Layer, processing data and applying business rules.
- ArtExplorer.DAL – Data Access Layer, handling database interactions.

### Prerequisites

### Before running the project locally, ensure you have the following installed:
- .NET SDK
- SQL Server or any compatible database
- Visual Studio or VS Code with C# extensions
- Postman (optional, for API testing)

### Getting Started

1. Clone the Repository
2. Update Connection String
   
3.  Modify the appsettings.json file in ArtExplorer.API:
 ````bash
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=ArtExplorer;"
}
````

For Entity Framework Core, update AppDbContext.cs in ArtExplorer.DAL:
 ````bash
optionsBuilder.UseSqlServer("Server=YOUR_SERVER;Database=ArtExplorer;");
````

4. Apply Database Migrations:
Run the following command in the ArtExplorer.API project directory:
 ````bash
dotnet ef database update
````
5. Run the Project:
 ````bash
dotnet run --project ArtExplorer.API
````
