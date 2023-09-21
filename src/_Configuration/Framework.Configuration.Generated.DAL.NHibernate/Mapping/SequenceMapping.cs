using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Configuration.Generated.DAL.NHibernate.Mapping
{
    public class SequenceMapping : CfgBaseMap<Sequence>
    {
        public SequenceMapping()
        {
            this.Map(x => x.Name).UniqueKey("UIX_nameSequence");
            this.Map(x => x.Number);
        }
    }
}
