using Framework.Authorization.Domain;
using Framework.DomainDriven.BLL;
using Framework.Exceptions;

namespace Framework.Authorization.BLL;

public partial class BusinessRoleBLL
{
    public BusinessRole GetAdminRole()
    {
        return this.GetByName(BusinessRole.AdminRoleName);
    }

    public override void Save(BusinessRole domainObject)
    {
        if (this.Context.CurrentPrincipal.RunAs != null)
        {
            throw new BusinessLogicException("RunAs mode must be disabled");
        }

        base.Save(domainObject);
    }

    public bool HasBusinessRole(string roleName, bool withRunAs = true)
    {
        if (roleName == null) throw new ArgumentNullException(nameof(roleName));

        return this.Context.AvailablePermissionSource.GetAvailablePermissionsQueryable(withRunAs)
                   .Any(permission => permission.Role.Name == roleName);
    }
}
