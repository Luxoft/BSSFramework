using Framework.DomainDriven.Auth;
using SecuritySystem.Credential;

namespace Automation.ServiceEnvironment.Services;

public interface IIntegrationTestUserAuthenticationService : IDefaultUserAuthenticationService
{
    void SetUser(UserCredential? customUserCredential);

    void Reset();

    Task<T> WithImpersonateAsync<T>(UserCredential customUserCredential, Func<Task<T>> func);
}
