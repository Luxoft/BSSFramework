using Framework.Authorization.Domain;

namespace Framework.Authorization.BLL;

public partial interface IPermissionFilterEntityBLL
{
    PermissionFilterEntity GetOrCreate(SecurityContextType entityType, SecurityEntity securityEntity, bool disableExistsCheck = false);
}
