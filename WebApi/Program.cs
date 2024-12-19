using DataLayer;
using DataLayer.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mapster;
using WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddMapster();

// Add services to the container.
// Register MovieDatabase with the dependency injection container
var connectionString = builder.Configuration.GetConnectionString("imdbDatabase");
if (string.IsNullOrEmpty(connectionString))
{
  throw new InvalidOperationException("The ConnectionString property has not been initialized.");
}
else
{
  Console.WriteLine($"Connection String: {connectionString}");
}

builder.Services.AddDbContext<MovieDbContext>(options =>
    options.UseNpgsql(connectionString));

// Register IDataService and the DataService implementation
builder.Services.AddSingleton<IDataService, DataService>();
builder.Services.AddSingleton(new Hashing());

//(Stored in appsettings.json)
var secret = builder.Configuration.GetSection("Auth:Secret").Value;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Look for the token in the cookies
                var cookie = context.HttpContext.Request.Cookies["auth_token_cookie"];
                if (!string.IsNullOrEmpty(cookie))
                {
                    context.Token = cookie;
                }
                // Since some browsers delete cookies, we also use tokens in local storage
                else if (string.IsNullOrEmpty(cookie) && context.HttpContext.Request.Headers.ContainsKey("Authorization"))
                {
                    // Check if the token is available in the Authorization header (for localStorage tokens)
                    var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
                    if (authHeader.StartsWith("Bearer "))
                    {
                        context.Token = authHeader.Substring("Bearer ".Length).Trim();
                    }
                }
                return Task.CompletedTask;
            }
        };
    });


// added CORS-service to apllow requests from React-app (localhost:5173)
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowReactApp", policy =>
  {
    policy.WithOrigins("http://localhost:5173", "http://localhost:3000") // URL to React-app
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod();
  });
});

builder.Services.AddScoped<IDataService, DataService>();

// Mapster configuration
builder.Services.AddMapster();

builder.Services.AddControllers();

// Swagger configuration for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage(); // Enable detailed error pages
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();


//This is where we apply the CORS-policy ( react has to be allowed to make requests to the API)
app.UseCors("AllowReactApp");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
