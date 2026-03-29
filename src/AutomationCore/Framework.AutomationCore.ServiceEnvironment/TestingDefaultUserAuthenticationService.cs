using CommonFramework.Auth;

using Framework.Application;
using Framework.Database;

using SecuritySystem.Testing;

namespace Framework.AutomationCore.ServiceEnvironment;

public class TestingDefaultCurrentUser(
    RootImpersonateServiceState rootImpersonateServiceState,
    TestRootUserInfo testRootUserInfo,
    IServiceEvaluator<ICurrentUser> currentUserEvaluator) : ICurrentUser
{
    public string Name =>

        rootImpersonateServiceState.CustomUserCredential == null
            ? testRootUserInfo.Name
            : rootImpersonateServiceState.Cache.TryGetValue(rootImpersonateServiceState.CustomUserCredential, out var cachedUserName)
                ? cachedUserName
                : currentUserEvaluator.Evaluate(DBSessionMode.Read, s => s.Name);
}
