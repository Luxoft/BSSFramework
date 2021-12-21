using System;
using Framework.Authorization.Domain;

namespace Framework.Authorization.BLL
{
    public partial interface IPermissionFilterEntityBLL
    {
        PermissionFilterEntity GetOrCreate(EntityType entityType, SecurityEntity securityEntity, bool disableExistsCheck = false);
    }
}