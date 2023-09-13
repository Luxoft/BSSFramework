using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Authorization.Generated.DAL.NHibernate.Mapping
{
    public class EntityTypeMapping : AuthBaseMap<EntityType>
    {
        public EntityTypeMapping()
        {
            this.Map(x => x.Name).Not.Nullable();
            this.Map(x => x.Expandable);
            this.Map(x => x.IsFilter);
        }
    }
}
