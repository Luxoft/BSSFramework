namespace Framework.SecuritySystem.ExternalSystem.ApplicationSecurity;

public static class ApplicationSecurityRule
{
    public static DomainSecurityRule.RoleFactorySecurityRule SecurityAdministrator { get; } = new(typeof(SecurityAdministratorRuleFactory));
}
