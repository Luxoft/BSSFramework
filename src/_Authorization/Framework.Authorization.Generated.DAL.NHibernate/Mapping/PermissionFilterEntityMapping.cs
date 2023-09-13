using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Authorization.Generated.DAL.NHibernate.Mapping
{
    public class PermissionFilterEntityMapping : AuthBaseMap<PermissionFilterEntity>
    {
        public PermissionFilterEntityMapping()
        {
            this.Map(x => x.EntityId)
                .Not.Nullable()
                .UniqueKey("UIX_entityId_entityTypePermissionFilterEntity");
            this.References(x => x.EntityType)
                .Column($"{nameof(PermissionFilterEntity.EntityType)}Id")
                .Not.Nullable()
                .UniqueKey("UIX_entityId_entityTypePermissionFilterEntity");
        }
    }
}
