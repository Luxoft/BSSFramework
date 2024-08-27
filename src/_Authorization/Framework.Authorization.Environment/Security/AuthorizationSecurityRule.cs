using Framework.SecuritySystem;

namespace Framework.Authorization.Environment.Security;

public static class AuthorizationSecurityRule
{
    public static DomainSecurityRule.ProviderSecurityRule CurrentPrincipal { get; } = new(typeof(CurrentPrincipalSecurityProvider<>));

    public static DomainSecurityRule.ProviderSecurityRule DelegatedFrom { get; } = new(typeof(DelegatedFromSecurityProvider<>));

    public static DomainSecurityRule.ProviderSecurityRule AvailableBusinessRole { get; } = new(typeof(AvailableBusinessRoleSecurityProvider<>));

    public static DomainSecurityRule.RoleFactorySecurityRule SecurityAdministrator { get; } = new(typeof(SecurityAdministratorRuleFactory));
}
