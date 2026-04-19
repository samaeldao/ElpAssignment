## 🚀 Getting Started

### Prerequisites
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* SQL Server (LocalDB or SQL Server Express)

### 1. Configuration
Ensure your `appsettings.Development.json` has a valid SQL Server connection string. By default, it expects a local instance.

### 2. Database Setup
The database schema and initial codebook seed data are managed via Entity Framework Core Migrations. Open your terminal at the solution root and run:

dotnet ef database update --project Elp.Infrastructure --startup-project Elp.Api

### 3. Run the API

dotnet run --project Elp.Api

### 4. Postman collection

The Postman collection is called "Elp.Api.postman_collection.json"

### 5. AI usage and details

The AI detailed explanation is written in "AI-Explanation.pdf"
