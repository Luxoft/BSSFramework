using System.Text.Encodings.Web;

using Framework.DomainDriven;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Framework.Cap.Auth;

public class CapAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IAuthorizationSystem authorizationSystem;

    private readonly IDBSession dbSession;

    public CapAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IAuthorizationSystem authorizationSystem,
            IDBSession dbSession)
            : base(options, logger, encoder, clock)
    {
        this.authorizationSystem = authorizationSystem;
        this.dbSession = dbSession;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var httpContext = this.Context;

        if (httpContext.User.Identity?.IsAuthenticated == false)
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        this.dbSession.AsReadOnly();

        if (!this.authorizationSystem.IsAdmin())
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var authenticationTicket = new AuthenticationTicket(httpContext.User, DependencyInjections.CapAuthenticationScheme);
        return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
    }
}
