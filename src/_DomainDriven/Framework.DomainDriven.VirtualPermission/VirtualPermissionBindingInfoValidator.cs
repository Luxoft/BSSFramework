using Framework.Core;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.VirtualPermission;

public class VirtualPermissionBindingInfoValidator(ISecurityRoleSource securityRoleSource) : IVirtualPermissionBindingInfoValidator
{
    private readonly HashSet<Guid> validated = new();

    public void Validate<TPrincipal, TPermission>(VirtualPermissionBindingInfo<TPrincipal, TPermission> bindingInfo)
    {
        if (this.validated.Contains(bindingInfo.Id))
        {
            return;
        }

        this.InternalValidate(bindingInfo);

        this.validated.Add(bindingInfo.Id);
    }

    private void InternalValidate<TPrincipal, TPermission>(VirtualPermissionBindingInfo<TPrincipal, TPermission> bindingInfo)
    {
        var securityContextRestrictions = securityRoleSource
                                          .GetSecurityRole(bindingInfo.SecurityRole)
                                          .Information
                                          .Restriction
                                          .SecurityContextRestrictions;

        if (securityContextRestrictions != null)
        {
            var bindingContextTypes = bindingInfo.GetSecurityContextTypes().ToList();

            var invalidTypes = bindingContextTypes.Except(securityContextRestrictions.Select(r => r.Type)).ToList();

            if (invalidTypes.Any())
            {
                throw new Exception($"Invalid restriction types: {invalidTypes.Join(", ", t => t.Name)}");
            }

            var missedTypes = securityContextRestrictions
                              .Where(r => r.Required)
                              .Select(r => r.Type)
                              .Except(bindingContextTypes)
                              .ToList();

            if (missedTypes.Any())
            {
                throw new Exception($"Missed required restriction types: {missedTypes.Join(", ", t => t.Name)}");
            }
        }
    }
}
