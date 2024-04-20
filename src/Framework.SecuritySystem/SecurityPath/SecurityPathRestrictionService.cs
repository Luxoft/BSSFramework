#nullable enable
namespace Framework.SecuritySystem;

public class SecurityPathRestrictionService : ISecurityPathRestrictionService
{
    public SecurityPath<TDomainObject> ApplyRestriction<TDomainObject>(SecurityPath<TDomainObject> securityPath, SecurityPathRestriction restriction)
    {
        return securityPath;
    }
}
