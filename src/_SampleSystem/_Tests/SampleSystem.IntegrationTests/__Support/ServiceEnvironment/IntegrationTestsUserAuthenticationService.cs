using System;
using System.Threading.Tasks;

using Framework.Core.Services;

namespace SampleSystem.IntegrationTests.__Support.ServiceEnvironment
{
    public class IntegrationTestsUserAuthenticationService : IUserAuthenticationService
    {
        private static readonly string DefaultUserName = $"{System.Environment.UserDomainName}\\{System.Environment.UserName}";

        public string GetUserName() => this.CustomUserName ?? DefaultUserName;

        public string CustomUserName { get; set; }

        public async Task<T> ImpersonateAsync<T>(string customUserName, Func<Task<T>> func)
        {
            var prev = this.CustomUserName;

            this.CustomUserName = customUserName;

            try
            {
                return await func();
            }
            finally
            {
                this.CustomUserName = prev;
            }
        }
    }
}
