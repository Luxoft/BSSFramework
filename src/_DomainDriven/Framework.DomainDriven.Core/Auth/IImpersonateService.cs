using Framework.SecuritySystem.Credential;

namespace Framework.DomainDriven.Auth;

public interface IImpersonateService
{
    Task<T> WithImpersonateAsync<T>(UserCredential? customUserCredential, Func<Task<T>> func);
}
