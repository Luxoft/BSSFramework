using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Configuration.Generated.DAL.NHibernate.Mapping
{
    public class ControlSettingsParamMapping : CfgBaseMap<ControlSettingsParam>
    {
        public ControlSettingsParamMapping()
        {
            this.Map(x => x.Type);
            this.References(x => x.ControlSettings).Column($"{nameof(ControlSettingsParam.ControlSettings)}Id");

            this.HasMany(x => x.ControlSettingsParamValues).AsSet().Inverse().Cascade.AllDeleteOrphan();
        }
    }
}
