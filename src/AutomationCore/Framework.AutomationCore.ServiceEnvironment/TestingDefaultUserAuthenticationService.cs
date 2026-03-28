using SecuritySystem;
using SecuritySystem.Testing;

namespace Framework.AutomationCore.ServiceEnvironment;

public class TestingDefaultUserAuthenticationService(
    RootImpersonateServiceState rootImpersonateServiceState,
    TestRootUserInfo testRootUserInfo,
    IServiceEvaluator<ICurrentUser> currentUserEvaluator) : IDefaultUserAuthenticationService
{
    public string GetUserName() =>

        rootImpersonateServiceState.CustomUserCredential == null
            ? testRootUserInfo.Name
            : rootImpersonateServiceState.Cache.TryGetValue(rootImpersonateServiceState.CustomUserCredential, out var cachedUserName)
                ? cachedUserName
                : currentUserEvaluator.Evaluate(DBSessionMode.Read, s => s.Name);
}
