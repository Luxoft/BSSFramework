using Automation.Settings;

using Framework.Core;
using Framework.DomainDriven;
using Framework.SecuritySystem.Credential;

using Microsoft.Extensions.Options;

namespace Automation.ServiceEnvironment.Services;

public class IntegrationTestUserAuthenticationService(
    IOptions<AutomationFrameworkSettings> settings,
    IServiceEvaluator<IUserCredentialNameResolver> credentialNameResolverEvaluator)
    : IIntegrationTestUserAuthenticationService
{
    private readonly IDictionaryCache<UserCredential, string> credCache = new DictionaryCache<UserCredential, string>(
        userCredential =>
        {
            return userCredential switch
            {
                UserCredential.NamedUserCredential v => v.Name,
                _ => credentialNameResolverEvaluator.Evaluate(DBSessionMode.Read, resolver => resolver.GetUserName(userCredential))
            };
        });

    private string IntegrationTestUserName => settings.Value.IntegrationTestUserName;

    public UserCredential? CustomUserCredential { get; internal set; }

    public void SetUserName(UserCredential? customUserCredential) =>
        this.CustomUserCredential = customUserCredential ?? this.IntegrationTestUserName;

    public void Reset() => this.CustomUserCredential = this.IntegrationTestUserName;

    public string GetUserName() =>
        this.CustomUserCredential == null ? this.IntegrationTestUserName : this.credCache[this.CustomUserCredential];

    public async Task<T> WithImpersonateAsync<T>(UserCredential customUserCredential, Func<Task<T>> func)
    {
        var prev = this.CustomUserCredential;

        this.CustomUserCredential = customUserCredential;

        try
        {
            return await func();
        }
        finally
        {
            this.CustomUserCredential = prev;
        }
    }
}
