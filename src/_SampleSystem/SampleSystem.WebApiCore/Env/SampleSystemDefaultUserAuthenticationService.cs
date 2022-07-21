using Framework.Core.Services;

using Microsoft.AspNetCore.Http;

namespace SampleSystem.WebApiCore.Env
{
    public class SampleSystemDefaultUserAuthenticationService : DomainDefaultUserAuthenticationService
    {

        private readonly IHttpContextAccessor httpContextAccessor;

        public SampleSystemDefaultUserAuthenticationService(IHttpContextAccessor httpContextAccessor) => this.httpContextAccessor = httpContextAccessor;

        public override string GetUserName() => this.httpContextAccessor.HttpContext?.User?.Identity?.Name ?? base.GetUserName();
    }
}
