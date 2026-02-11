using Framework.DomainDriven.Auth;

using SecuritySystem.Testing;

namespace Automation.ServiceEnvironment;

public class TestingDefaultUserAuthenticationService(ITestingUserAuthenticationService testingUserAuthenticationService) : IDefaultUserAuthenticationService
{
    public string GetUserName() => testingUserAuthenticationService.GetUserName();
}
