using System.Transactions;

using Framework.Core.Services;

using Microsoft.AspNetCore.Http;

namespace SampleSystem.WebApiCore.Env
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public UserAuthenticationService(IHttpContextAccessor httpContextAccessor) => this.httpContextAccessor = httpContextAccessor;

        public string GetUserName() => CurrentUser;

        public static string CurrentUser { get; } = $"{System.Environment.UserDomainName}\\{System.Environment.UserName}";
    }
}
