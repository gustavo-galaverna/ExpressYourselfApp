# Interview Assignment - Novibet ExpressYourself

## Description

This repository contains the implementation of an application that manages IP information and their respective countries. The application uses a SQL Server database, a .NET API, and a Redis server for caching. The Redis server and the application are configured to run in Docker containers.

## Features

- Get IP Details: The API exposes an endpoint that returns details for a specific IP (Country Name, Two and Three Letter Country Code).
- Caching: The application attempts to retrieve IP information from the Redis cache. If not found, it queries the database. If the data is not in the database, it makes a call to the IP2C service, persists the data in both the database and the cache.
- Periodic Data Update: Every hour, the application updates the IP and country information in the database and cache if necessary.

## Technologies Used

- **Framework/Stack**: .NET 9 - Framework used to build the API.
- **Database**: SQL Server - Database used to persist IP and country information.
- **Cache**: Redis - Caching system to improve performance and reduce costs of external calls.
- **Container**: Docker - For containerizing the application and Redis.

## How To Run The Application

### Prerequisites

Before running the application, make sure to have the following installed: 

- Docker (to run the containers)
- .NET SDK 9.0 or above
- SQL Server (local)


### How to run

1. Clone this repository:
   ```bash
   git clone https://github.com/gustavo-galaverna/ExpressYourselfApp.git
   
2. Browse to projects directory: 
```
cd ExpressYourselfApp
```
3. Restore dependencies: 
```
dotnet restore
```
4. Configure a SQL Server Database

5. Run Docker Containers: 
```
docker-compose up --build
```
6. Access the API

Access the API
Once the containers are built and started, the application will be available at http://localhost:8080/swagger/index.html, as configured in the Dockerfile / docker-compose



### API Endpoints

GET /api/ip/details - Returns details of the given Ip Address

POST /api/ip/report - Returns a report of the given countries or all countries in case country codes are null

### Project Structure

- /ExpressYourself.API: Contains the API controllers.
- /ExpressYourself.Application: Contains the business logic (Services).
- /ExpressYourself.Infrastructure: Contains the repositories and database configuration.
- /ExpressYourself.Domain: Contains domain entities and interfaces.
- /ExpressYourself.Persistence: Contains the database connection setup and repositories.

### Tests

Tests can be run with the following command: 
```
dotnet test
```

### Final Considerations

This project was developed to demonstrate the ability to integrate various systems like SQL Server, Redis, and Docker, as well as implementing good development practices such as caching and periodic data updates.

