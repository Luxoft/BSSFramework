using Framework.Authorization.Domain;

namespace Framework.Authorization.BLL;

public partial interface IBusinessRoleBLL
{
    BusinessRole GetByNameOrCreate(string name, bool autoSave = false);

    BusinessRole GetAdminRole();

    BusinessRole GetOrCreateAdminRole();

    bool HasAdminRole();

    bool HasBusinessRole(string roleName, bool withRunAs = true);

    IEnumerable<BusinessRoleNode> GetNodes();

    BusinessRole Save(BusinessRoleNode businessRoleNode);

    IEnumerable<BusinessRole> GetParents(ICollection<BusinessRole> businessRoles);
}
