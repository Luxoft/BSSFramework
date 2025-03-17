namespace Framework.SecuritySystem.DependencyInjection;

public class InitializedSecurityRoleSource(IEnumerable<PreInitializerFullSecurityRole> securityRoles) : IInitializedSecurityRoleSource
{
    public IEnumerable<FullSecurityRole> GetSecurityRoles()
    {
        return securityRoles.Select(sr => this.GetInitializedRole(sr.FullSecurityRole));
    }

    protected virtual IReadOnlyList<SecurityRole> ExceptAdministratorRoles { get; } = [SecurityRole.Administrator, SecurityRole.SystemIntegration];

    private FullSecurityRole GetInitializedRole(FullSecurityRole securityRole)
    {
        if (securityRole == SecurityRole.Administrator)
        {
            var info = securityRole.Information;

            var otherRoles = securityRoles.Select(sr => sr.FullSecurityRole).Except(this.ExceptAdministratorRoles);

            var newInfo = securityRole.Information with
                          {
                              Children = info.Children.Concat(otherRoles).Distinct().ToList(),
                              Restriction = SecurityPathRestriction.Ignored
                          };

            return new FullSecurityRole(securityRole.Name, newInfo);
        }
        else if (securityRole == SecurityRole.SystemIntegration)
        {
            var newInfo = securityRole.Information with { Restriction = SecurityPathRestriction.Ignored };

            return new FullSecurityRole(securityRole.Name, newInfo);
        }
        else
        {
            return securityRole;
        }
    }
}
