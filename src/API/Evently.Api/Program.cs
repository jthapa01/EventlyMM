using Evently.Api.Extensions;
using Evently.Modules.Events.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEventsModule(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    ILoggerFactory loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
    ILogger logger = loggerFactory.CreateLogger("MigrationLogger");
    app.ApplyMigrations(logger);
}

EventsModule.MapEndpoints(app);

app.Run();
