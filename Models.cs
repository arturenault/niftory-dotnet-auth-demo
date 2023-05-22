using System.Text.Json.Serialization;

record DeviceCodeEndpointResponse {
    [JsonPropertyName("device_code")]
    public string? DeviceCode { get; init;}

    [JsonPropertyName("user_code")]
    public string? UserCode { get; init;}


    [JsonPropertyName("verification_uri")]
    public string? VerificationUri { get; init;}

    [JsonPropertyName("verification_uri_complete")]
    public string? VerificationUriComplete { get; init;}

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; init;}
}

record DeviceCodeTokenEndpointResponse {
    [JsonPropertyName("error")]
    public string? Error {get;init;}


    [JsonPropertyName("error_description")]
    public string? ErrorDescription {get;init;}

    [JsonPropertyName("access_token")]
    public string? AccessToken {get;init;}

    [JsonPropertyName("id_token")]
    public string? IdToken {get;init;}

    [JsonPropertyName("refresh_token")]
    public string? RefreshToken {get;init;}

    [JsonPropertyName("scope")]
    public string? Scope {get;init;}

    [JsonPropertyName("token_type")]
    public string? Bearer {get;init;}
}