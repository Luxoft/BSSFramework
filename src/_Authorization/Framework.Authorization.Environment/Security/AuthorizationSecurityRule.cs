using SecuritySystem;

namespace Framework.Authorization.Environment.Security;

public static class AuthorizationSecurityRule
{
    public static DomainSecurityRule.ProviderSecurityRule AvailableBusinessRole { get; } = new(typeof(AvailableBusinessRoleSecurityProvider<>));
}
