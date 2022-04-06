using Framework.Core.Services;

namespace SampleSystem.IntegrationTests.__Support.ServiceEnvironment
{
    public class IntegrationTestsUserAuthenticationService : IUserAuthenticationService
    {
        private IntegrationTestsUserAuthenticationService()
        {
        }

        private static readonly string DefaultUserName = $"{System.Environment.UserDomainName}\\{System.Environment.UserName}";

        public string GetUserName() => this.CustomUserName ?? DefaultUserName;

        public string CustomUserName { get; set; }

        public static IntegrationTestsUserAuthenticationService Instance = new IntegrationTestsUserAuthenticationService();
    }

}
