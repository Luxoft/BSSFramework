using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Configuration.Generated.DAL.NHibernate.Mapping;

public class CodeFirstSubscriptionMap : ConfigurationBaseMap<CodeFirstSubscription>
{
    public CodeFirstSubscriptionMap() =>
        this.Map(x => x.Code).Length(512).UniqueKey("UIX_codeCodeFirstSubscription").Not.Nullable();
}
