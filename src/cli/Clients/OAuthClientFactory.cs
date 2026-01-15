using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;

namespace Cli.Clients;

/// <summary>
/// Options for the OAuth client factory
/// </summary>
public record OAuthClientFactoryOptions
{
  /// <summary>
  /// The client ID for the OAuth client
  /// </summary>
  public required string ClientId { get; init; }
  /// <summary>
  /// The URL to use for the authorization request
  /// </summary>
  public required Uri AuthorizationUrl { get; init; }
  /// <summary>
  /// The URL to use for the token request
  /// </summary>
  public required Uri TokenUrl { get; init; }
}

/// <summary>
/// A response from the OAuth provider containing a device code
/// </summary>
public record OAuthDeviceCodeResponse
{
  [JsonPropertyName("device_code")]
  public required string DeviceCode { get; init; }
  [JsonPropertyName("user_code")]
  public required string UserCode { get; init; }
  [JsonPropertyName("verification_uri")]
  public required Uri VerificationUri { get; init; }
  [JsonPropertyName("interval")]
  public required int Interval { get; init; }

  public override string ToString()
  {
    return $"DeviceCode: {DeviceCode}, UserCode: {UserCode}, VerificationUri: {VerificationUri}, Interval: {Interval}";
  }
}

/// <summary>
/// A response from the OAuth provider containing an access token
/// </summary>
public record TokenResponse
{
  [JsonPropertyName("access_token")]
  public required string AccessToken { get; init; }
  [JsonPropertyName("token_type")]
  public required string TokenType { get; init; }
  [JsonPropertyName("expires_in")]
  public required int ExpiresIn { get; init; }
}

/// <summary>
/// A factory for OAuth clients
/// </summary>
public class OAuthClientFactory
{
  private readonly OAuthClientFactoryOptions _options;
  private readonly ILogger<OAuthClientFactory> _logger;
  private TokenResponse? _tokenResponse;

  /// <summary>
  /// Initializes a new instance of the OAuthClientFactory class
  /// </summary>
  /// <param name="options">The options for the OAuth client factory</param>
  /// <param name="logger">The logger to use for the OAuth client factory</param>
  public OAuthClientFactory(OAuthClientFactoryOptions options, ILogger<OAuthClientFactory> logger)
  {
    _options = options;
    _logger = logger;
  }

  /// <summary>
  /// Creates a new HttpClient with the Authorization header set to the access token
  /// </summary>
  /// <returns>A new HttpClient with the Authorization header set to the access token</returns>
  /// <exception cref="InvalidOperationException">Thrown if the client is not logged in</exception>
  public HttpClient CreateAuthenticatedClient() => new()
  {
    DefaultRequestHeaders = {
      { "Authorization", $"Bearer {_tokenResponse?.AccessToken ?? throw new InvalidOperationException("Client not logged in")}" },
    },
  };

  /// <summary>
  /// Authenticates with the OAuth provider
  /// </summary>
  /// <returns>True if the login was successful, false otherwise</returns>
  public async Task<bool> LoginAsync()
  {
    var client = new HttpClient();

    var responseContent = await RequestDeviceCodeAsync(client);

    // Inform the user to login at the verification URL
    var message = """
      Please login at {VerificationUri}
      User code: {UserCode}
      Waiting for authorization to complete...
      """;
    _logger.LogInformation(message, responseContent.VerificationUri, responseContent.UserCode);

    var result = await PollForTokenAsync(client, responseContent);

    if (result)
    {
      _logger.LogInformation("Authorization complete");
      return true;
    }
    else
    {
      _logger.LogError("Authorization failed");
      return false;
    }
  }

  /// <summary>
  /// Requests a device code from the OAuth provider
  /// </summary>
  /// <param name="client">The HttpClient to use for the request</param>
  /// <returns>The device code response</returns>
  private async Task<OAuthDeviceCodeResponse> RequestDeviceCodeAsync(HttpClient client)
  {
    _logger.LogInformation("Requesting device code...");
    var request = new FormUrlEncodedContent(new Dictionary<string, string>
    {
      { "client_id", _options.ClientId },
      { "response_type", "device_code" },
      { "scope", "openid profile" },
    });

    var response = await client.PostAsync(_options.AuthorizationUrl, request);
    response.EnsureSuccessStatusCode();

    var responseContent = await response.Content.ReadFromJsonAsync<OAuthDeviceCodeResponse>();

    if (responseContent == null)
    {
      _logger.LogError("Failed to get device code response: {Response}", await response.Content.ReadAsStringAsync());
      throw new Exception("Failed to get device code response");
    }

    return responseContent;
  }

  /// <summary>
  /// Polls the OAuth provider for a token
  /// </summary>
  /// <param name="client">The HttpClient to use for the request</param>
  /// <param name="responseContent">The device code response</param>
  /// <returns>True if the token was successfully retrieved, false otherwise</returns>
  private async Task<bool> PollForTokenAsync(HttpClient client, OAuthDeviceCodeResponse responseContent)
  {
    var request = new FormUrlEncodedContent(new Dictionary<string, string>
    {
      { "client_id", _options.ClientId },
      { "grant_type", "urn:ietf:params:oauth:grant-type:device_code" },
      { "device_code", responseContent.DeviceCode },
    });

    while (true)
    {
      var tokenResponse = await client.PostAsync(_options.TokenUrl, request);
      var tokenResponseContent = await tokenResponse.Content.ReadFromJsonAsync<JsonElement>();
      // The returned JSON is a disjoint set of possible types, the simplest is to check these sequentially

      if (tokenResponseContent.TryGetProperty("error", out var error) && error.GetString() == "authorization_pending")
      {
        _logger.LogInformation("Authorization pending, waiting for {Interval} seconds", responseContent.Interval);
        await Task.Delay(responseContent.Interval * 1000);
        continue;
      }

      if (tokenResponseContent.TryGetProperty("access_token", out var accessToken) && accessToken.GetString() != null)
      {
        // We now know that the authorization is of the TokenResponse type
        _tokenResponse = JsonSerializer.Deserialize<TokenResponse>(tokenResponseContent.GetRawText());
        return true;
      }

      _logger.LogError("Failed to get token response: {TokenResponse}", PrettyPrintJson(tokenResponseContent));
      throw new Exception("Failed to get token response");
    }
  }

  public string GetToken()
  {
    return _tokenResponse?.AccessToken ?? throw new Exception("Login first!");
  }

  /// <summary>
  /// Pretty prints a JSON element
  /// </summary>
  /// <param name="json">The JSON element to pretty print</param>
  /// <returns>A string representation of the JSON element</returns>
  private static string PrettyPrintJson(JsonElement json)
  {
    return JsonSerializer.Serialize(json, new JsonSerializerOptions { WriteIndented = true });
  }
}
