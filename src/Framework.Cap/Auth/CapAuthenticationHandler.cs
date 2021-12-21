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
    private readonly IServiceEnvironment<TBllContext> environment;

    public CapAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IServiceEnvironment<TBllContext> environment)
            : base(options, logger, encoder, clock) =>
            this.environment = environment;

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var httpContext = this.Context;

        if (httpContext.User.Identity?.IsAuthenticated == false)
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var isAdmin = this.environment
                          .GetContextEvaluator()
                          .Evaluate(
                                    DBSessionMode.Read,
                                    z => z.Authorization.Logics.BusinessRole.HasAdminRole());

        if (!isAdmin)
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var authenticationTicket = new AuthenticationTicket(
                                                            httpContext.User,
                                                            DependencyInjections
                                                                    .CapAuthenticationScheme);
        return Task.FromResult(AuthenticateResult.Success(authenticationTicket));

    }
}
