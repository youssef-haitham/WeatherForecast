# Weather Forecast API

A clean and minimal ASP.NET Core Web API that provides weather forecasts for any city.  
This project demonstrates JWT authentication, HttpOnly cookies, in-memory storage, caching, and a simple modular architecture.

## Features

- JWT authentication using HttpOnly cookies  
- Register, login, and logout endpoints  
- Authenticated weather endpoint: `/api/weather?city=CityName`  
- Mocked weather data generated through a service/repository pattern  
- Per-city caching using `IMemoryCache`  
- Clear separation of concerns (Controllers, Services, Repositories, Providers)

## Generating a JWT Secret Key

Before running the API, generate a secure 64-byte Base64 key and add it to `appsettings.Development.json`.
Run the following PowerShell command:

```powershell
[System.Convert]::ToBase64String((1..64 | ForEach-Object {Get-Random -Minimum 0 -Maximum 256}))
```

Place the generated key inside:

```json
"Jwt": {
  "Key": "<YOUR-GENERATED-KEY>",
  "Issuer": "WeatherApi",
  "Audience": "WeatherApiUsers",
  "ExpirationHours": 24
}
```

## Running the Project

### Running from Visual Studio

Select the **swagger** profile from the run configuration dropdown in Visual Studio, then press **F5**.

This profile is configured in `launchSettings.json` to automatically open the Swagger UI at:

```
https://localhost:7277/swagger
```

### Running from the CLI

1. Restore dependencies:
   ```
   dotnet restore
   ```
2. Run the API:
   ```
   dotnet run
   ```
3. Open Swagger manually at:
   ```
   https://localhost:<port>/swagger
   ```

## API Endpoints

### Authentication

#### Register  
`POST /api/auth/signup`

Request body:
```json
{
  "name": "Test User",
  "email": "test@example.com",
  "password": "123456"
}
```

#### Login  
`POST /api/auth/signin`  
Returns a JWT stored inside an `auth_token` HttpOnly cookie.

#### Logout  
`POST /api/auth/logout`  
Removes the authentication cookie.

### Weather

#### Get Weather by City  
`GET /api/weather?city=Cairo`  
Requires authentication.

Returns mocked weather data with randomized temperature and conditions.  
Weather data is cached per city for improved performance.
