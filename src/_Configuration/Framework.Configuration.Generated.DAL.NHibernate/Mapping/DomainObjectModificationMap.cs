using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Configuration.Generated.DAL.NHibernate.Mapping;

public class DomainObjectModificationMap : ConfigurationBaseMap<DomainObjectModification>
{
    public DomainObjectModificationMap()
    {
        this.Map(x => x.Type);
        this.Map(x => x.Processed);
        this.Map(x => x.DomainObjectId)
            .Not.Nullable()
            .UniqueKey("UIX_domainObjectId_domainType_revisionDomainObjectModification");
        this.Map(x => x.Revision)
            .Not.Nullable()
            .UniqueKey("UIX_domainObjectId_domainType_revisionDomainObjectModification");
        this.References(x => x.DomainType).Column($"{nameof(DomainObjectModification.DomainType)}Id")
            .Not.Nullable()
            .UniqueKey("UIX_domainObjectId_domainType_revisionDomainObjectModification");

        this.Version(x => x.Version).Generated.Never().Not.Nullable();
    }
}
