using System.Net.Http.Json;

namespace Evently.Modules.Users.Infrastructure.Identity;

internal sealed class KeyCloakClient(HttpClient httpClient)
{
    internal async Task<string> RegisterUserAsync(UserRepresentation user, CancellationToken cancellationToken = default)
    {
        HttpResponseMessage httpResponseMessage = await httpClient.PostAsJsonAsync("users", user, cancellationToken);
        
        httpResponseMessage.EnsureSuccessStatusCode();
        
        return ExtractIdentityIdFromLocationHeader(httpResponseMessage);
    }

    private static string ExtractIdentityIdFromLocationHeader(HttpResponseMessage httpResponseMessage)
    {
        const string usersSegmentName = "users/";
        
        string locationHeader = httpResponseMessage.Headers.Location?.PathAndQuery
            ?? throw new InvalidOperationException("Location header is missing.");

        int userSegmentIndex = locationHeader.IndexOf(usersSegmentName, StringComparison.OrdinalIgnoreCase);
        string identityId = locationHeader[(userSegmentIndex + usersSegmentName.Length)..];

        return identityId;
    }
}
