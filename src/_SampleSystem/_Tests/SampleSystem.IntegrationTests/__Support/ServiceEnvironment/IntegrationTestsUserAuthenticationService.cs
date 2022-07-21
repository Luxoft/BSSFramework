using System;
using System.Threading.Tasks;

using Framework.Core.Services;
using Framework.DomainDriven.ServiceModel.IAD;

namespace SampleSystem.IntegrationTests.__Support.ServiceEnvironment
{
    public class IntegrationTestsUserAuthenticationService : IUserAuthenticationService, IImpersonateService
    {
        private static readonly string DefaultUserName = $"{System.Environment.UserDomainName}\\{System.Environment.UserName}";

        public string GetUserName() => CustomUserName ?? DefaultUserName;

        public static string CustomUserName { get; private set; }

        public async Task<T> WithImpersonateAsync<T>(string customUserName, Func<Task<T>> func)
        {
            var prev = CustomUserName;

            CustomUserName = customUserName;

            try
            {
                return await func();
            }
            finally
            {
                CustomUserName = prev;
            }
        }
    }
}
