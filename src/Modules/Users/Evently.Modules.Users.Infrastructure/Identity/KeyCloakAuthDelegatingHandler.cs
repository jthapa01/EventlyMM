using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;

namespace Evently.Modules.Users.Infrastructure.Identity;

internal sealed class KeyCloakAuthDelegatingHandler(IOptions<KeyCloakOptions> keyCloakOptions) : DelegatingHandler
{
    private readonly KeyCloakOptions _keyCloakOptions = keyCloakOptions.Value;
    
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        AuthToken authToken = await GetAuthorizationToken(cancellationToken);
        
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken.AccessToken);
        
        HttpResponseMessage httpResponseMessage = await base.SendAsync(request, cancellationToken);

        httpResponseMessage.EnsureSuccessStatusCode();
        
        return httpResponseMessage;
    }

    private async Task<AuthToken> GetAuthorizationToken(CancellationToken cancellationToken)
    {
        var authRequestParameters = new KeyValuePair<string, string>[]
        {
            new("client_id", _keyCloakOptions.ConfidentialClientId),
            new("client_secret", _keyCloakOptions.ConfidentialClientSecret),
            new("scope", "openid"),
            new("grant_type", "client_credentials")
        };
        
        using var authRequestContent = new FormUrlEncodedContent(authRequestParameters);
        using var authRequest = new HttpRequestMessage(HttpMethod.Post, new Uri(_keyCloakOptions.TokenUrl));
        authRequest.Content = authRequestContent;
        using HttpResponseMessage authorizationResponse = await base.SendAsync(authRequest, cancellationToken);
        authorizationResponse.EnsureSuccessStatusCode();
        return await authorizationResponse.Content.ReadFromJsonAsync<AuthToken>(cancellationToken: cancellationToken);
    }
    private sealed class AuthToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; init; }
    }
}
