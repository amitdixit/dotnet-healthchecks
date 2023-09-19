using HealthCheckApp.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DataAccess;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
// Add services to the container.

builder.Services.AddHealthChecks()
        .AddCheck<DatabaseHealthCheck>("Database")
        .AddSqlServer(config.GetConnectionString("StudentAppContext"))
        .AddCheck<HttpApiHealthCheck>("HttpApi");

builder.Services.AddHttpClient();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

builder.Services.AddDbContext<StudentAppContext>(options =>
{
    options.UseSqlServer(config.GetConnectionString("StudentAppContext"));
});

var app = builder.Build();

//Seed Dummy Data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<StudentAppContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var appLogger = services.GetRequiredService<ILogger<Program>>();
        appLogger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/_health", options: new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseAuthorization();

app.MapControllers();

app.Run();
