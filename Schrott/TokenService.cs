using Microsoft.Identity.Client;
using System.Text.Json;

namespace Schrott;

public static class TokenService
{
    public static async Task<string> GetAccessToken(FormUrlEncodedContent content, string tokenEndpoint)
    {
        var client = new HttpClient();
        var response = await client.PostAsync(tokenEndpoint, content).ConfigureAwait(continueOnCapturedContext: false);
        var jsonString = await response.Content.ReadAsStringAsync();
        client.Dispose();

        var doc = JsonDocument.Parse(jsonString);
        JsonElement root = doc.RootElement;
        if (root.TryGetProperty("access_token", out JsonElement tokenElement))
            return tokenElement.GetString()!;

        throw new Exception("Failed to get access token");
    }

    public static async Task<string> GetAccessTokenO365(string tenantId, string clientId, string secret, IEnumerable<string> scopes)
    {
        var confidentialClientApplication = ConfidentialClientApplicationBuilder.Create(clientId)
        .WithAuthority($"https://login.microsoftonline.com/{tenantId}/v2.0")
        .WithClientSecret(secret)
        .Build();

        var authToken = await confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();
        return authToken.AccessToken;
    }
}
