using System.Diagnostics;
using RestSharp;

var authIssuer = "http://auth.test.niftory.com";
var client = new RestClient(authIssuer);

// Start the OAuth process by querying the device_authorization_endpoint
var request = new RestRequest("/oidc/device/auth");
request.AddHeader("content-type", "application/x-www-form-urlencoded");
var clientId = "NIFTORY_CLI";
var scope = "openid email profile offline_access admin";
request.AddParameter("application/x-www-form-urlencoded", $"client_id={clientId}&scope={scope}&prompt=consent", ParameterType.RequestBody);
var response = await client.PostAsync<DeviceCodeEndpointResponse>(request);

// Display the verification_uri_complete to the user. You can also do verification_uri, but you'll need to display the user_code so they can type that in.
Console.WriteLine(response!.VerificationUriComplete);

var stopwatch = new Stopwatch();
stopwatch.Start();

DeviceCodeTokenEndpointResponse? token = null;
while (stopwatch.Elapsed.TotalSeconds < response.ExpiresIn)
{
    await Task.Delay(5000);

    // Poll the token_endpoint to see if the user has authenticated yet.
    var tokenRequest = new RestRequest("/oidc/token");
    tokenRequest.AddHeader("content-type", "application/x-www-form-urlencoded");
    tokenRequest.AddParameter("application/x-www-form-urlencoded", $"client_id={clientId}&device_code={response.DeviceCode}&grant_type=urn%3Aietf%3Aparams%3Aoauth%3Agrant-type%3Adevice_code", ParameterType.RequestBody);

    var tokenResponse = await client.ExecutePostAsync<DeviceCodeTokenEndpointResponse>(tokenRequest);
    if (tokenResponse.Data!.IdToken != null)
    {
        token = tokenResponse.Data;
        break;
    }

    if (tokenResponse.Data.Error != null && tokenResponse.Data.Error != "authorization_pending")
    {
        throw new Exception($"Error from token endpoint: {tokenResponse.Data.Error} - {tokenResponse.Data.ErrorDescription}");
    }
}

Console.WriteLine($"Initial token: {token.IdToken}");

// Refresh the token just to show that it works.
var refreshRequest = new RestRequest("/oidc/token");
refreshRequest.AddHeader("content-type", "application/x-www-form-urlencoded");
refreshRequest.AddParameter("application/x-www-form-urlencoded", $"grant_type=refresh_token&client_id={clientId}&refresh_token={token.RefreshToken}", ParameterType.RequestBody);

var refreshResponse = await client.ExecutePostAsync<DeviceCodeTokenEndpointResponse>(refreshRequest);
Console.WriteLine($"Refreshed token: {refreshResponse.Data!.IdToken}");

