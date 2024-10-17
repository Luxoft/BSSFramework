using Framework.Core.Services;
using Framework.SecuritySystem.Credential;

namespace Framework.SecuritySystem;

public class UserNameResolver(
    ICurrentUser currentUser,
    IUserAuthenticationService userAuthenticationService,
    IUserCredentialNameResolver userCredentialNameResolver) : IUserNameResolver
{
    public string? Resolve(SecurityRuleCredential credential)
    {
        switch (credential)
        {
            case SecurityRuleCredential.CustomUserSecurityRuleCredential customUserSecurityRuleCredential:
                return userCredentialNameResolver.GetUserName(customUserSecurityRuleCredential.UserCredential);

            case SecurityRuleCredential.CurrentUserWithRunAsCredential:
                return currentUser.Name;

            case SecurityRuleCredential.CurrentUserWithoutRunAsCredential:
                return userAuthenticationService.GetUserName();

            case SecurityRuleCredential.AnyUserCredential:
                return null;

            default:
                throw new ArgumentOutOfRangeException(nameof(credential));
        }
    }
}
