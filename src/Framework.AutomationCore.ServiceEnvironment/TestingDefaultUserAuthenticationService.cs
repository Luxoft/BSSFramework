using CommonFramework;

using Framework.DomainDriven.Auth;

using SecuritySystem.Services;
using SecuritySystem.Testing;

namespace Automation.ServiceEnvironment;

public class TestingDefaultUserAuthenticationService(
    ITestingEvaluator<IRawUserAuthenticationService> userAuthenticationServiceEvaluator,
    IDefaultCancellationTokenSource? defaultCancellationTokenSource = null) : IDefaultUserAuthenticationService
{
    public string GetUserName() =>
        defaultCancellationTokenSource.RunSync(async _ => await userAuthenticationServiceEvaluator.EvaluateAsync(TestingScopeMode.Read, async s => s.GetUserName()));
}
