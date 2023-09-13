using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Configuration.Generated.DAL.NHibernate.Mapping
{
    public class NamedLockMapping : CfgBaseMap<NamedLock>
    {
        public NamedLockMapping()
        {
            this.Map(x => x.LockOperation);
        }
    }
}
