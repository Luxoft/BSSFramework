using Framework.Authorization.Domain;

namespace Framework.Authorization.BLL;

public partial interface IBusinessRoleBLL
{
    BusinessRole GetAdminRole();

    bool HasBusinessRole(string roleName, bool withRunAs = true);
}
