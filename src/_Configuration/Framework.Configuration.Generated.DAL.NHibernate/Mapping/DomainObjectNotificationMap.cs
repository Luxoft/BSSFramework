using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Configuration.Generated.DAL.NHibernate.Mapping;

public class DomainObjectNotificationMap : ConfigurationBaseMap<DomainObjectNotification>
{
    public DomainObjectNotificationMap()
    {
        this.Map(x => x.HostName);
        this.Map(x => x.ProcessDate);
        this.Map(x => x.SerializeData).Length(int.MaxValue).Not.Nullable();
        this.Map(x => x.Size);
        this.Map(x => x.Status);
    }
}
