namespace SteynPJM.CustomerProjects.Common.Models
{
  public class JwtOptions
  {
    public string SigningKey { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public string Issuer { get; set; } = string.Empty;

    public int AccessTokenExpirationSeconds { get; set; } = 600;

  }
}
