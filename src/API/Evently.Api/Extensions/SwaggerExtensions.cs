using Microsoft.OpenApi.Models;

namespace Evently.Api.Extensions;

internal static class SwaggerExtensions
{
    internal static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Evently API",
                Version = "v1",
                Description = "Evently API built using the modular monolith architecture.",
            });
            options.CustomSchemaIds(type => type.FullName?.Replace("+", "."));
        });

        return services;
    }
}
