#nullable enable
namespace Framework.SecuritySystem;

public interface ISecurityPathRestrictionService
{
    SecurityPath<TDomainObject> ApplyRestriction<TDomainObject>(
        SecurityPath<TDomainObject> securityPath,
        SecurityPathRestriction restriction);
}
