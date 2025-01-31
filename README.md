# BookStoreBackend

BookStoreBackend is a backend application for managing a bookstore, developed in C# using Entity Framework.

## Requirements

- .NET SDK version 8.0 or later
- Database server (e.g., SQL Server, PostgreSQL)

## Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/0nufriy/BookStoreBackend.git
   cd BookStoreBackend
   ```

2. Open the solution in your preferred development environment (e.g., Visual Studio, Rider).

3. Configure the database connection string in `appsettings.json`:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "YourConnectionStringHere"
     }
   }
   ```

4. Apply migrations to create the necessary tables in the database:

   ```bash
   dotnet ef database update
   ```

5. Run the application through the IDE

## Usage

Once the application is running, it will be available at `https://localhost:7109` (or another port specified in the settings). You can interact with the API using tools like Postman or cURL.
