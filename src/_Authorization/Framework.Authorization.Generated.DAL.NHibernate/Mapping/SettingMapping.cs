using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Authorization.Generated.DAL.NHibernate.Mapping
{
    public class SettingMapping : AuthBaseMap<Setting>
    {
        public SettingMapping()
        {
            this.Map(x => x.Key);
            this.Map(x => x.Value);
        }
    }
}
