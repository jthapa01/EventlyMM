using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Evently.Api.Extensions;
using Evently.Api.Middleware;
using Evently.Api.OpenTelemetry;
using Evently.Common.Application;
using Evently.Common.Infrastructure;
using Evently.Common.Infrastructure.Configuration;
using Evently.Common.Presentation.Endpoints;
using Evently.Modules.Attendance.Infrastructure;
using Evently.Modules.Events.Infrastructure;
using Evently.Modules.Ticketing.Infrastructure;
using Evently.Modules.Users.Infrastructure;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Npgsql;
using Serilog;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();

Assembly[] moduleApplicationAssemblies = [
    Evently.Modules.Users.Application.AssemblyReference.Assembly,
    Evently.Modules.Events.Application.AssemblyReference.Assembly,
    Evently.Modules.Ticketing.Application.AssemblyReference.Assembly,
    Evently.Modules.Attendance.Application.AssemblyReference.Assembly];

builder.Services.AddApplication(moduleApplicationAssemblies);

string databaseConnectionString = builder.Configuration.GetConnectionStringOrThrow("Database");
string redisConnectionString = builder.Configuration.GetConnectionStringOrThrow("Cache");

builder.Services.AddInfrastructure(
    DiagnosticsConfig.ServiceName,
    [
        EventsModule.ConfigureConsumers(redisConnectionString),
        TicketingModule.ConfigureConsumers,
        AttendanceModule.ConfigureConsumers
    ],
    databaseConnectionString,
    redisConnectionString);

Uri keyCloakHealthUrl = builder.Configuration.GetKeyCloakHealthUrl();

builder.Services.AddHealthChecks()
    .AddNpgSql(databaseConnectionString)
    .AddRedis(redisConnectionString)
    .AddKeyCloak(keyCloakHealthUrl);

builder.Configuration.AddModuleConfiguration(["users", "events", "ticketing", "attendance"],builder.Environment.EnvironmentName);

builder.Services.AddEventsModule(builder.Configuration);

builder.Services.AddUsersModule(builder.Configuration);

builder.Services.AddTicketingModule(builder.Configuration);

builder.Services.AddAttendanceModule(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Docker"))
{
    app.UseSwagger();
    app.UseSwaggerUI();

    RetryDatabaseConnection(app, 3, TimeSpan.FromSeconds(10));
}

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseLogContext();

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

app.UseAuthentication();

app.UseAuthorization();

app.MapEndpoints();

app.Run();


static void RetryDatabaseConnection(WebApplication app, int maxRetries, TimeSpan delay)
{
    int retryCount = 0;
    string databaseConnectionString = app.Configuration.GetConnectionString("Database");

    while (retryCount < maxRetries)
    {
        try
        {
            using var connection = new NpgsqlConnection(databaseConnectionString);
            connection.Open();
            app.ApplyMigrations();
            break;
        }
        catch (Exception ex)
        {
            retryCount++;
            if (retryCount >= maxRetries)
            {
                throw new Exception("Could not connect to the database after multiple attempts.", ex);
            }

            Thread.Sleep(delay);
        }
    }
}

[SuppressMessage("Design", "CA1515:Types should be made internal", Justification = "This class needs to be public for integration tests.")]
public partial class Program;
