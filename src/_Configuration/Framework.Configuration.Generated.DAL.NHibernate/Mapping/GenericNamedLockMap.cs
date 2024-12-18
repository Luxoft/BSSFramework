using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Configuration.Generated.DAL.NHibernate.Mapping;

public class GenericNamedLockMap : ConfigurationBaseMap<GenericNamedLock>
{
    public GenericNamedLockMap() => this.Map(x => x.Name).UniqueKey("UIX_nameGenericNamedLock").Not.Nullable();
}
