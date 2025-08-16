using FluentValidation;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using RestroLogic.Domain.Exceptions;
using RestroLogic.Infrastructure.Persistence;
using Serilog;
using RestroLogic.Application;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();
builder.Host.UseSerilog();

// EF Core(SQL Server o InMemory)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sql => sql.EnableRetryOnFailure())); // UseInMemoryDatabase para pruebas

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(ApplicationAssembly.Value));

// FluentValidation
builder.Services.AddValidatorsFromAssembly(ApplicationAssembly.Value);

// ProblemDetails
builder.Services.AddProblemDetails(opts =>
{
    opts.Map<DomainException>(ex => new Microsoft.AspNetCore.Mvc.ProblemDetails
    {
        Title = "Regla de negocio violada",
        Status = StatusCodes.Status422UnprocessableEntity,
        Detail = ex.Message,
        Type = "https://httpstatuses.com/422"
    });
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSerilogRequestLogging();
app.UseProblemDetails();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Seeder
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(db);
}

app.MapControllers();
app.Run();
