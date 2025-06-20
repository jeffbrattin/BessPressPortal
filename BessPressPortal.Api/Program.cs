using BessPressPortal.Api.Controllers.BessPressPortal.Server.Controllers;
using BessPressPortal.Api.Data;
using BessPressPortal.Api.Entities;
using BessPressPortal.Api.Services;
using BessPressPortal.Shared.Models.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BessPressPortal API", Version = "v1" });
});

// Configure CORS to allow Blazor WebAssembly client
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorClient", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
        else
        {
            policy.WithOrigins("https://besspressportalclient.blueground-271272db.westus2.azurecontainerapps.io")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
    });
});

var connString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Using connection string: {connString}");


// Configure Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<UserInfo, IdentityRole<int>>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();


// Configure JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key not found in appsettings.json");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

//-----------------------
//Lifetime Created	Shared?	Example Use
//Singleton	Once per app	Yes (globally)	Config readers, static caches, stateless helpers
//Scoped	Once per request	Yes (within request)	Database contexts, user-specific logic
//Transient	New every time	No	Lightweight, disposable services

builder.Services.AddSingleton<NotesService>();
builder.Services.AddScoped<AuthenticationTableService>();
builder.Services.AddScoped<IPasswordHasher<LoginEntity>, PasswordHasher<LoginEntity>>();
builder.Services.AddSingleton<ITestRepository>(sp => new TestRepository(
builder.Configuration.GetConnectionString("DefaultConnection")));
// Update the code to handle the potential null value for the connection string

//-----------------------

if (string.IsNullOrEmpty(connString))
{
    throw new InvalidOperationException("DefaultConnection not found in appsettings.json");
}

// Configure Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connString));

// Configure TestRepository with a null check
builder.Services.AddSingleton<ITestRepository>(sp =>
{
    if (string.IsNullOrEmpty(connString))
    {
        throw new InvalidOperationException("DefaultConnection not found in appsettings.json");
    }
    return new TestRepository(connString);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("BlazorClient"); // Apply CORS before Authentication/Authorization
//app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // If using API controllers
    endpoints.MapGet("/", () => "API is running"); // Optional default response
});


app.Run();