using Framework.Authorization.Domain;

namespace Framework.Authorization.BLL;

public partial class PermissionFilterEntityBLL
{
    public PermissionFilterEntity GetOrCreate(EntityType entityType, SecurityEntity securityEntity, bool disableExistsCheck = false)
    {
        if (entityType == null) throw new ArgumentNullException(nameof(entityType));
        if (securityEntity == null) throw new ArgumentNullException(nameof(securityEntity));

        var existsEntity = this.GetListBy(filterEntity => filterEntity.EntityType == entityType && filterEntity.EntityId == securityEntity.Id)
                               .SingleOrDefault();

        if (existsEntity != null)
        {
            return existsEntity;
        }

        return this.Context.ExternalSource.GetTyped(entityType).AddSecurityEntity(securityEntity, disableExistsCheck);
    }
}
