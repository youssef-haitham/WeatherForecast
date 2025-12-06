using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using WeatherForecast.Api.Interfaces.Providers;
using WeatherForecast.Api.Interfaces.Repositories;
using WeatherForecast.Api.Interfaces.Services;
using WeatherForecast.Api.Providers;
using WeatherForecast.Api.Repositories;
using WeatherForecast.Api.Services;
using WeatherForecast.Api.Settings;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

//Services DI
services.AddScoped<IAuthService, AuthService>();
services.AddScoped<IWeatherService, WeatherService>();
//Repositories DI
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IWeatherRepository, WeatherRepository>();
//Providers DI
services.AddScoped<IHashProvider, HashProvider>();
services.AddScoped<ITokenProvider, TokenProvider>();

services.AddMemoryCache();

services.AddControllers();
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    JwtSettings? jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;
    options.TokenValidationParameters = new TokenValidationParameters
    {

        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
                Convert.FromBase64String(jwtSettings.Key)
            ),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.TryGetValue("auth_token", out var token))
            {
                context.Token = token;
            }

            return Task.CompletedTask;
        }
    };
});

services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Weather Forecast API");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();