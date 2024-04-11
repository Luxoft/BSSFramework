using Framework.Authorization.Domain;

namespace Framework.Authorization.BLL;

public partial class PermissionFilterEntityBLL
{
    public PermissionFilterEntity GetOrCreate(SecurityContextType securityContextType, SecurityEntity securityEntity, bool disableExistsCheck = false)
    {
        if (securityContextType == null) throw new ArgumentNullException(nameof(securityContextType));
        if (securityEntity == null) throw new ArgumentNullException(nameof(securityEntity));

        var existsEntity = this.GetListBy(filterEntity => filterEntity.EntityType == securityContextType && filterEntity.EntityId == securityEntity.Id)
                               .SingleOrDefault();

        if (existsEntity != null)
        {
            return existsEntity;
        }

        return this.Context.ExternalSource.GetTyped(securityContextType).AddSecurityEntity(securityEntity, disableExistsCheck);
    }
}
