using FluentValidation;
using FluentValidation.AspNetCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestroLogic.Application.Dtos.Products;
using RestroLogic.Application.Interfaces;
using RestroLogic.Application.Services.Orders;
using RestroLogic.Application.Services.Products;
using RestroLogic.Domain.Interfaces;
using RestroLogic.Infrastructure.Interfaces;
using RestroLogic.Infrastructure.Persistence;
using RestroLogic.Infrastructure.Repositories;
using RestroLogic.Infrastructure.Storage;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var jwt = builder.Configuration.GetSection("Jwt");
var keyBytes = Encoding.UTF8.GetBytes(jwt["Key"]!);

// EF Core(SQL Server o InMemory)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // UseInMemoryDatabase para pruebas

// Repositorios y servicios
builder.Services.Configure<AzureBlobOptions>(
builder.Configuration.GetSection("AzureBlob"));

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IImageStorage, AzureBlobImageStorage>();

// Controllers y Swagger/OpenAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(CreateProductDto).Assembly);

builder.Services.AddProblemDetails(options =>
{
    options.Map<ValidationException>(ex =>
        new StatusCodeProblemDetails(StatusCodes.Status422UnprocessableEntity)
        {
            Title = "Validation failed",
            Detail = string.Join(" | ", ex.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"))
        });

    options.Map<ArgumentException>(ex =>
        new StatusCodeProblemDetails(StatusCodes.Status400BadRequest)
        {
            Title = "Invalid argument",
            Detail = ex.Message
        });

    options.Map<KeyNotFoundException>(ex =>
        new StatusCodeProblemDetails(StatusCodes.Status404NotFound)
        {
            Title = "Not found",
            Detail = ex.Message
        });

    options.MapToStatusCode<NotImplementedException>(StatusCodes.Status501NotImplemented);
    options.MapToStatusCode<UnauthorizedAccessException>(StatusCodes.Status401Unauthorized);
    options.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
});

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwt["Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(2)
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", p => p.RequireRole("admin"));
    options.AddPolicy("Kitchen", p => p.RequireRole("kitchen", "admin"));
    options.AddPolicy("Waiter", p => p.RequireRole("waiter", "admin"));
});

var app = builder.Build();

app.UseProblemDetails();

// Middleware y mapeo de endpoints
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
