using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Exceptions;

namespace Framework.Authorization.BLL;

public partial class BusinessRoleBLL
{
    public BusinessRole GetOrCreateAdminRole()
    {
        return this.GetByNameOrCreate(BusinessRole.AdminRoleName, true);
    }

    public BusinessRole GetAdminRole()
    {
        return this.GetByName(BusinessRole.AdminRoleName);
    }

    public BusinessRole GetByNameOrCreate(string name, bool autoSave = false)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));

        return this.GetByName(name) ?? new BusinessRole { Name = name }.Self(autoSave, this.Save);
    }

    public override void Save(BusinessRole domainObject)
    {
        if (this.Context.CurrentPrincipal.RunAs != null)
        {
            throw new BusinessLogicException("RunAs mode must be disabled");
        }

        base.Save(domainObject);
    }
    
    public bool HasAdminRole()
    {
        var adminRole = this.GetAdminRole();

        return adminRole != null
               && this.HasBusinessRole(adminRole.Name);
    }

    public bool HasBusinessRole(string roleName, bool withRunAs = true)
    {
        if (roleName == null) throw new ArgumentNullException(nameof(roleName));

        return this.Context.AvailablePermissionSource.GetAvailablePermissionsQueryable(withRunAs)
                   .Any(permission => permission.Role.Name == roleName);
    }

    protected override void PostValidate(BusinessRole businessRole, AuthorizationOperationContext operationContext)
    {
        if (businessRole == null) throw new ArgumentNullException(nameof(businessRole));

        this.ValidateDelegatePermissions(businessRole);
    }

    private void ValidateDelegatePermissions(BusinessRole businessRole)
    {
        var permissionBLL = this.Context.Logics.Permission;

        permissionBLL.GetListBy(permission => permission.Role == businessRole).Foreach(p => permissionBLL.ValidatePermissionDelegated(p, ValidatePermissonDelegateMode.Role));
    }

    public BusinessRole Create(BusinessRoleCreateModel createModel)
    {
        if (createModel == null) throw new ArgumentNullException(nameof(createModel));

        return new BusinessRole();
    }
}
