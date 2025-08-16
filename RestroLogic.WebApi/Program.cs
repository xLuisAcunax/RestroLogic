using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RestroLogic.Application.Commands.Sales.AddItem;
using RestroLogic.Application.Commands.Sales.CloseOrder;
using RestroLogic.Application.Commands.Sales.CreateOrder;
using RestroLogic.Application.Commands.Sales.GetOrders;
using RestroLogic.Application.Common.Behaviors;
using RestroLogic.Application.Common.Mapping;
using RestroLogic.Infrastructure.Persistence;
using RestroLogic.WebApi.Contracts;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Serilog
Log.Logger = new LoggerConfiguration()
.Enrich.FromLogContext()
.Enrich.WithEnvironmentUserName()
.WriteTo.Console()
.CreateLogger();

builder.Host.UseSerilog();

// EF Core(SQL Server o InMemory)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // UseInMemoryDatabase para pruebas

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateOrderCommand>());

// Behaviors
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

// FluentValidation auto-registrado por assembly scanning (si usas DI extensions)

// Mapster
MappingConfig.Register();

// ProblemDetails (RFC7807)
builder.Services.AddProblemDetails(options =>
{
    options.IncludeExceptionDetails = (ctx, ex) => builder.Environment.IsDevelopment();
});

builder.Services.AddHealthChecks();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseProblemDetails();

// Middleware y mapeo de endpoints
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHealthChecks("/health");


app.MapPost("/api/v1/orders", async (CreateOrderCommand cmd, IMediator mediator, AppDbContext db, CancellationToken ct) =>
{
    var result = await mediator.Send(cmd, ct);
    return result.IsSuccess ? Results.Created($"/api/v1/orders/{result.Value}", new { id = result.Value }) : Results.BadRequest(new { error = result.Error });
});

app.MapPost("/api/v1/orders/{id:guid}/items", async (Guid id, AddItemRequest req, IMediator mediator, CancellationToken ct) =>
{
    var cmd = new AddItemCommand(id, req.MenuItemId, req.MenuItemName, req.UnitPrice, req.Currency ?? "COP", req.Quantity);
    var result = await mediator.Send(cmd, ct);
    return result.IsSuccess ? Results.Ok(result.Value) : Results.BadRequest(new { error = result.Error });
});


app.MapPost("/api/v1/orders/{id:guid}/close", async (Guid id, IMediator mediator, CancellationToken ct) =>
{
    var result = await mediator.Send(new CloseOrderCommand(id), ct);
    return result.IsSuccess ? Results.Ok(new { total = result.Value }) : Results.BadRequest(new { error = result.Error });
});


app.MapGet("/api/v1/orders", async (string? status, int page, int pageSize, IMediator mediator, HttpResponse http, CancellationToken ct) =>
{
    var res = await mediator.Send(new GetOrdersQuery(status, page == 0 ? 1 : page, pageSize == 0 ? 20 : pageSize), ct);
    if (!res.IsSuccess) return Results.BadRequest(new { error = res.Error });


    var meta = new { res.Value.Page, res.Value.PageSize, res.Value.TotalCount, res.Value.TotalPages };
    http.Headers["X-Pagination"] = System.Text.Json.JsonSerializer.Serialize(meta);
    return Results.Ok(res.Value);
});

// Seed DB at startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.SeedAsync(db);
}

//app.UseHttpsRedirection();
//app.UseAuthentication();
//app.UseAuthorization();
//app.MapControllers();

app.Run();
