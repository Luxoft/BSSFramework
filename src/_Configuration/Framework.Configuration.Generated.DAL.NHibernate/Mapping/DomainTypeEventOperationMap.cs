using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Configuration.Generated.DAL.NHibernate.Mapping;

public class DomainTypeEventOperationMap : ConfigurationBaseMap<DomainTypeEventOperation>
{
    public DomainTypeEventOperationMap()
    {
        this.Map(x => x.Name)
            .UniqueKey("UIX_domainType_nameDomainTypeEventOperation")
            .Not.Nullable();
        this.References(x => x.DomainType).Column($"{nameof(DomainTypeEventOperation.DomainType)}Id")
            .UniqueKey("UIX_domainType_nameDomainTypeEventOperation")
            .Not.Nullable();
    }
}
