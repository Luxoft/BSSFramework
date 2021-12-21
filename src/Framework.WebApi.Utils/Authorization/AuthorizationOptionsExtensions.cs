using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Server.HttpSys;

namespace Framework.WebApi.Utils
{
    public static class AuthorizationOptionsExtensions
    {
        public static AuthorizationOptions AddNTLMAuthentication(this AuthorizationOptions options)
        {
            var policy = new AuthorizationPolicyBuilder()
                         .AddAuthenticationSchemes(nameof(AuthenticationSchemes.NTLM))
                         .RequireAuthenticatedUser()
                         .Build();

            options.AddPolicy(nameof(AuthenticationSchemes.NTLM), policy);
            return options;
        }
    }
}
