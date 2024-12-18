using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Configuration.Generated.DAL.NHibernate.Mapping;

public class TargetSystemMap : ConfigurationBaseMap<TargetSystem>
{
    public TargetSystemMap()
    {
        this.Map(x => x.IsBase);
        this.Map(x => x.IsMain);
        this.Map(x => x.IsRevision);
        this.Map(x => x.Name).UniqueKey("UIX_nameTargetSystem").Not.Nullable();
        this.Map(x => x.SubscriptionEnabled);
        this.HasMany(x => x.DomainTypes).AsSet().Inverse().Cascade.AllDeleteOrphan();
    }
}
