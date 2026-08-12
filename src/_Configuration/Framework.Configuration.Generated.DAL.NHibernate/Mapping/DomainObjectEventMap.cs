using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Configuration.Generated.DAL.NHibernate.Mapping;

public class DomainObjectEventMap : ConfigurationBaseMap<DomainObjectEvent>
{
    public DomainObjectEventMap()
    {
        this.Map(x => x.DomainObjectId);
        this.Map(x => x.HostName);
        this.Map(x => x.ProcessDate);
        this.Map(x => x.QueueTag).Not.Nullable();
        this.Map(x => x.Revision);
        this.Map(x => x.SerializeData).Length(int.MaxValue);
        this.Map(x => x.SerializeType).Length(int.MaxValue);
        this.Map(x => x.Size);
        this.Map(x => x.Status);
        this.References(x => x.Operation).Column($"{nameof(DomainObjectEvent.Operation)}Id");
    }
}
