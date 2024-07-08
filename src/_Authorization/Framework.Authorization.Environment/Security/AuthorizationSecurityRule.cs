using Framework.SecuritySystem;

namespace Framework.Authorization.Environment.Security;

public static class AuthorizationSecurityRule
{
    public static SecurityRule.CustomProviderSecurityRule CurrentPrincipal { get; } = new(typeof(PrincipalSecurityProvider<>));

    public static SecurityRule.CustomProviderSecurityRule DelegatedFrom { get; } = new(typeof(DelegatedFromSecurityProvider<>));

    public static SecurityRule.CustomProviderSecurityRule AvailableBusinessRole { get; } = new(typeof(AvailableBusinessRoleSecurityProvider<>));
}
