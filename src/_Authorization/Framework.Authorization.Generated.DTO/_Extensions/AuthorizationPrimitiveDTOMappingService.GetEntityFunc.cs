using System;
using System.Collections.Generic;
using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;

namespace Framework.Authorization.Generated.DTO
{
    public partial class AuthorizationServerPrimitiveDTOMappingService
    {
        private Func<EntityTypeIdentityDTO, SecurityEntityIdentityDTO, PermissionFilterEntity> _getEntityFunc;

        private Func<EntityTypeIdentityDTO, SecurityEntityIdentityDTO, PermissionFilterEntity> GetEntityFunc
        {
            get
            {
                if (this._getEntityFunc == null)
                {
                    this._getEntityFunc = FuncHelper.Create((EntityTypeIdentityDTO entityTypeIdent, SecurityEntityIdentityDTO securityEntityIdent) =>
                    {
                        var entityType = this.GetById<EntityType>(entityTypeIdent.Id, IdCheckMode.CheckAll);

                        return this.Context.Logics.PermissionFilterEntity.GetOrCreate(entityType, securityEntityIdent.ToDomainObject());
                    }).WithCache();
                }

                return this._getEntityFunc;
            }
        }
    }
}
