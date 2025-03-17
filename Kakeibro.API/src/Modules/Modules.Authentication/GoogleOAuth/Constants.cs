namespace Modules.Authentication.GoogleOAuth;

public static class Constants
{
    public static class OAuthQueryParams
    {
        public const string ClientId = "client_id";
        public const string RedirectUri = "redirect_uri";
        public const string ResponseType = "response_type";
        public const string Scope = "scope";
        public const string AccessType = "access_type";
        public const string Prompt = "prompt";
    }

    public static class OAuthQueryParamValues
    {
        public const string ResponseType = "code";
        public const string Scope = "openid email profile";
        public const string AccessType = "offline";
        public const string Prompt = "consent";
    }
}
