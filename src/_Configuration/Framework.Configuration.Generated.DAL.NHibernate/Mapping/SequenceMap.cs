using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Configuration.Generated.DAL.NHibernate.Mapping;

public class SequenceMap : ConfigurationBaseMap<Sequence>
{
    public SequenceMap()
    {
        this.Map(x => x.Name).UniqueKey("UIX_nameSequence").Not.Nullable();
        this.Map(x => x.Number);
    }
}
