using System.Net.Http.Json;
using System.Text.Json;
using Cli.Clients;
using Microsoft.Extensions.Logging;

// Set up logging
using var loggerFactory = LoggerFactory.Create(builder =>
{
  builder
    .AddConsole()
    .SetMinimumLevel(LogLevel.Warning);
});
var logger = loggerFactory.CreateLogger<Program>();

// Set up the OAuth client
var oauthClientFactory = new OAuthClientFactory(new OAuthClientFactoryOptions
{
  ClientId = "355629494930863364",
  AuthorizationUrl = new Uri("http://localhost:8080/oauth/v2/device_authorization"),
  TokenUrl = new Uri("http://localhost:8080/oauth/v2/token"),
}, loggerFactory.CreateLogger<OAuthClientFactory>());

// Log in to the OAuth provider
logger.LogInformation("Logging in...");
await oauthClientFactory.LoginAsync();

// Create an authenticated client
var client = oauthClientFactory.CreateAuthenticatedClient();

// Make some authenticated requests
var response = await client.GetAsync("http://localhost:8080/oidc/v1/userinfo");
response.EnsureSuccessStatusCode();

var responseContent = await Utilities.PrettifyJsonAsync(response.Content);
logger.LogInformation("Response: {Response}", responseContent);



/// <summary>
/// Utility methods for the program
/// </summary>
public record Utilities
{
  /// <summary>
  /// Prettifies a JSON element
  /// </summary>
  /// <param name="content">The HttpContent to prettify</param>
  /// <returns>A string representation of the JSON element</returns>
  public static async Task<string> PrettifyJsonAsync(HttpContent content)
  {
    return JsonSerializer.Serialize(await content.ReadFromJsonAsync<JsonElement>(), new JsonSerializerOptions { WriteIndented = true });
  }
}
