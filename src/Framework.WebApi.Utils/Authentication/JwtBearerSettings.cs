namespace Framework.WebApi.Utils
{
    internal sealed class JwtBearerSettings
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }
    }
}
