using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Configuration.Generated.DAL.NHibernate.Mapping
{
    public class CodeFirstSubscriptionMapping : CfgBaseMap<CodeFirstSubscription>
    {
        public CodeFirstSubscriptionMapping()
        {
            this.Map(x => x.Code).Length(512)
                .UniqueKey("UIX_codeCodeFirstSubscription")
                .Not.Nullable();
        }
    }
}
