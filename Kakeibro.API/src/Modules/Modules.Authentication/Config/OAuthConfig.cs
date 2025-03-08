namespace Modules.Authentication.Config;

public class OAuthConfig
{
    public string? ClientId { get; set; }
    public string? RedirectUri { get; set; }
    public string? JavaScriptOrigin { get; set; }
    public string? AuthorizationEndpoint { get; set; }
    public string? TokenEndpoint { get; set; }
    public string? UserInfoEndpoint { get; set; }
    public string? RevokeTokenEndpoint { get; set; }
    public string? RefreshTokenEndpoint { get; set; }
}
