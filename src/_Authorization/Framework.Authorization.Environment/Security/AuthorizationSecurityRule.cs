using Framework.SecuritySystem;

namespace Framework.Authorization.Environment.Security;

public static class AuthorizationSecurityRule
{
    public static SecurityRule.ProviderSecurityRule CurrentPrincipal { get; } = new(typeof(PrincipalSecurityProvider<>));

    public static SecurityRule.ProviderSecurityRule DelegatedFrom { get; } = new(typeof(DelegatedFromSecurityProvider<>));

    public static SecurityRule.ProviderSecurityRule AvailableBusinessRole { get; } = new(typeof(AvailableBusinessRoleSecurityProvider<>));
}
