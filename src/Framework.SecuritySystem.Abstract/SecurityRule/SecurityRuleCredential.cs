using Framework.SecuritySystem.Credential;

namespace Framework.SecuritySystem;

public abstract record SecurityRuleCredential
{
    public record CurrentUserWithRunAsCredential : SecurityRuleCredential;

    public record CurrentUserWithoutRunAsCredential : SecurityRuleCredential;

    public record AnyUserCredential : SecurityRuleCredential;

    public record CustomUserSecurityRuleCredential(UserCredential UserCredential) : SecurityRuleCredential;
}
