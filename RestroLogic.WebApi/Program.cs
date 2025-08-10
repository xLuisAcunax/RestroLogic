using Microsoft.EntityFrameworkCore;
using RestroLogic.Application.Interfaces;
using RestroLogic.Application.Services.Orders;
using RestroLogic.Application.Services.Products;
using RestroLogic.Domain.Interfaces;
using RestroLogic.Infrastructure.Interfaces;
using RestroLogic.Infrastructure.Persistence;
using RestroLogic.Infrastructure.Repositories;
using RestroLogic.Infrastructure.Storage;

var builder = WebApplication.CreateBuilder(args);

// EF Core(SQL Server o InMemory)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // o UseInMemoryDatabase para pruebas

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

var app = builder.Build();

// Middleware y mapeo de endpoints
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
