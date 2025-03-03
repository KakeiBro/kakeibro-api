namespace Modules.Authentication.GoogleOAuth;

public interface IGoogleOAuthRepository
{
    void GenerateGoogleOAuthToken(string clientId, string clientSecret);
}

public class GoogleOAuthRepository : IGoogleOAuthRepository
{
    public void GenerateGoogleOAuthToken(string clientId, string clientSecret)
    {
        Console.WriteLine("Generating Google OAuth Token...");
    }
}
