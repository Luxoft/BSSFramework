using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Configuration.Generated.DAL.NHibernate.Mapping;

public class ControlSettingsParamMap : ConfigurationBaseMap<ControlSettingsParam>
{
    public ControlSettingsParamMap()
    {
        this.Map(x => x.Type);
        this.References(x => x.ControlSettings).Column($"{nameof(ControlSettingsParam.ControlSettings)}Id").Not.Nullable();
        this.HasMany(x => x.ControlSettingsParamValues).AsSet().Inverse().Cascade.AllDeleteOrphan();
    }
}
