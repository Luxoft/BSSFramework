using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Authorization.Generated.DAL.NHibernate.Mapping
{
    public class PermissionFilterItemMapping : AuthBaseMap<PermissionFilterItem>
    {
        public PermissionFilterItemMapping()
        {
            this.Map(x => x.ContextEntityId).Not.Nullable();
            this.References(x => x.EntityType).Column($"{nameof(PermissionFilterItem.EntityType)}Id")
                .Not.Nullable();
            this.References(x => x.Entity).Column($"{nameof(PermissionFilterItem.Entity)}Id")
                .Not.Nullable().UniqueKey("UIX_entity_permissionPermissionFilterItem");
            this.References(x => x.Permission).Column($"{nameof(PermissionFilterItem.Permission)}Id")
                .Not.Nullable().UniqueKey("UIX_entity_permissionPermissionFilterItem");
        }
    }
}
