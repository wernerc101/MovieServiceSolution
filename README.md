# Movie Service API

This project is a C# ASP.NET Core Web API that wraps the OMDB API, providing a structured way to search for and manage movie data. It includes caching mechanisms, data persistence with MySQL, and a console application for user interaction.

## Features

- **Swagger Integration**: Automatically generated API documentation for easy testing and exploration of endpoints.
- **OMDB API Wrapper**: Simplifies interactions with the OMDB API using defined DTOs.
- **In-Memory Caching**: Caches API responses to improve performance and reduce redundant API calls.
- **MySQL Data Persistence**: Stores cached entries in a MySQL database for durability.
- **OData Support**: Enables flexible querying of cached entries through OData.
- **CRUD Operations**: Full support for creating, reading, updating, and deleting cached entries.

## Project Structure

- **MovieService.Api**: The main API project containing controllers, services, and data access layers.
- **MovieService.Console**: A console application that interacts with the API, allowing users to manage cached entries.
- **MovieService.Common**: A shared library containing common models used across the API and console applications.

## Setup Instructions

1. **Clone the Repository**:
   ```
   git clone <repository-url>
   cd MovieServiceSolution
   ```

2. **Configure MySQL**:
   - Ensure you have a MySQL server running.
   - Update the connection string in `MovieService.Api/appsettings.json` to point to your MySQL database.

3. **Install Dependencies**:
   - Navigate to each project directory and run:
     ```
     dotnet restore
     ```

4. **Run the API**:
   - Start the API project:
     ```
     cd MovieService.Api
     dotnet run
     ```
   - Access the Swagger UI at `http://localhost:<port>/swagger` to explore the API.

5. **Run the Console Application**:
   - In a new terminal, navigate to the console project:
     ```
     cd MovieService.Console
     dotnet run
     ```
   - Follow the prompts to interact with the cached entries.

## Usage

- Use the console application to perform operations such as searching for movies, creating new cached entries, updating existing entries, and deleting entries.
- The API can also be tested directly through the Swagger UI.

## Contributing

Contributions are welcome! Please submit a pull request or open an issue for any enhancements or bug fixes.

## License

This project is licensed under the MIT License. See the LICENSE file for details.