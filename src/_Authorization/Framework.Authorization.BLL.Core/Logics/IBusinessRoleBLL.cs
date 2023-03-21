using System.Collections.Generic;

using Framework.Authorization.Domain;

using JetBrains.Annotations;

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

    IEnumerable<BusinessRole> GetParents([NotNull] ICollection<BusinessRole> businessRoles);
}
