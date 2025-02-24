using Evently.Common.Infrastructure.Configuration;

namespace Evently.Api.Extensions;

internal static class KeyCloakHealthChecksBuilderExtensions
{
    private const string KeyCloakHealthCheck = "Keycloak";
    private const string KeyCloakHealthUrl = "KeyCloak:HealthUrl";
    
    internal static IHealthChecksBuilder AddKeyCloak(this IHealthChecksBuilder builder, Uri healthUri)
    {
        builder.AddUrlGroup(healthUri, HttpMethod.Get,KeyCloakHealthCheck);
        
        return builder;
    }
    
    internal static Uri GetKeyCloakHealthUrl(this IConfiguration configuration)
        => new Uri(configuration.GetValueOrThrow<string>(KeyCloakHealthUrl));
}
