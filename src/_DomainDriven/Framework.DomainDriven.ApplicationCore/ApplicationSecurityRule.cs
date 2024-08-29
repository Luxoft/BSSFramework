using Framework.SecuritySystem;

namespace Framework.DomainDriven.ApplicationCore;

public static class ApplicationSecurityRule
{
    public static DomainSecurityRule.RoleFactorySecurityRule SecurityAdministrator { get; } = new(typeof(SecurityAdministratorRuleFactory));
}
