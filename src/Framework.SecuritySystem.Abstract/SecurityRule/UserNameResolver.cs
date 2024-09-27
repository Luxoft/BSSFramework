using Framework.Core.Services;
using Framework.SecuritySystem.UserSource;

namespace Framework.SecuritySystem;

public class UserNameResolver(ICurrentUser currentUser, IUserAuthenticationService userAuthenticationService) : IUserNameResolver
{
    public string? Resolve(SecurityRuleCredential credential)
    {
        switch (credential)
        {
            case SecurityRuleCredential.CustomUserSecurityRuleCredential customUserSecurityRuleCredential:
                return customUserSecurityRuleCredential.Name;

            case not null when credential == SecurityRuleCredential.CurrentUserWithRunAs:
                return currentUser.Name;

            case not null when credential == SecurityRuleCredential.CurrentUserWithoutRunAs:
                return userAuthenticationService.GetUserName();

            case not null when credential == SecurityRuleCredential.AnyUser:
                return null;

            default:
                throw new ArgumentOutOfRangeException(nameof(credential));
        }
    }
}
