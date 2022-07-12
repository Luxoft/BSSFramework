using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using Framework.Authorization.BLL;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.ServiceModel.Service;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Framework.Cap.Auth;

public class CapAuthenticationHandler<TBllContext> : AuthenticationHandler<AuthenticationSchemeOptions>
        where TBllContext : IAuthorizationBLLContextContainer<IAuthorizationBLLContext>
{
    private readonly TBllContext context;

    private readonly IDBSession dbSession;

    public CapAuthenticationHandler(
            IServiceProvider rootServiceProvider,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            TBllContext context,
            IDBSession dbSession)
            : base(options, logger, encoder, clock)
    {
        this.context = context;
        this.dbSession = dbSession;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var httpContext = this.Context;

        if (httpContext.User.Identity?.IsAuthenticated == false)
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        //this.dbSession.AsReadOnly();

        var isAdmin = this.context.Authorization.Logics.BusinessRole.HasAdminRole();

        if (!isAdmin)
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var authenticationTicket = new AuthenticationTicket(httpContext.User, DependencyInjections.CapAuthenticationScheme);
        return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
    }
}
