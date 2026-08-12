using Anch.SecuritySystem;

namespace Framework.Authorization.Environment.Security;

public static class AuthorizationSecurityRule
{
    public static DomainSecurityRule.ProviderSecurityRule AvailableBusinessRole { get; } = new() { GenericSecurityProviderType = typeof(AvailableBusinessRoleSecurityProvider<>) };
}
