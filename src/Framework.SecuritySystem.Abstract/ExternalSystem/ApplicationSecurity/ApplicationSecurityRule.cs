using Framework.SecuritySystem;

namespace Framework.DomainDriven.ApplicationSecurity;

public static class ApplicationSecurityRule
{
    public static DomainSecurityRule.RoleFactorySecurityRule SecurityAdministrator { get; } = new(typeof(SecurityAdministratorRuleFactory));
}
