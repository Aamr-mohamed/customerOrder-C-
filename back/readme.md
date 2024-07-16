# Asp.net Backend

This repository contains the backend API server for Customer Orders System, built using ASP.NET Core and Entity Framework Core.

## Prerequisites

Before you begin, ensure you have the following installed:

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Visual Studio](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/) with C# extension

## Installation

1. **Clone the repository:**

   ```bash
   git clone <repository_url>
   cd backend
   ```

2. **Restore packages:**
   ```bash
   dotnet restore
   ```

3. **Apply EF Core Migrations:**
   ```bash
   dotnet ef database update
   ```

## Running the Application
Run the backend server:

```bash
dotnet run
```
Or
```bash
dotnet watch
```

Access the API:

Once the application is running, you can access the API endpoints at https://localhost:5022/swagger

