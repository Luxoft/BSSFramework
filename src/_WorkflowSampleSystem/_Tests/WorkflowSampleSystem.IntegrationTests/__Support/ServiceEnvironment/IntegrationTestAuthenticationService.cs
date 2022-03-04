using Framework.Core.Services;

namespace WorkflowSampleSystem.IntegrationTests.__Support.ServiceEnvironment
{
    public class IntegrationTestAuthenticationService : IUserAuthenticationService
    {
        private IntegrationTestAuthenticationService()
        {
        }

        public string GetUserName() => $"{System.Environment.UserDomainName}\\{System.Environment.UserName}";

        public static readonly IntegrationTestAuthenticationService Instance = new();
    }
}
