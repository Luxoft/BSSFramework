using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Configuration.Generated.DAL.NHibernate.Mapping
{
    public class ControlSettingsParamValueMapping : CfgBaseMap<ControlSettingsParamValue>
    {
        public ControlSettingsParamValueMapping()
        {
            this.Map(x => x.Culture);
            this.Map(x => x.Value).Length(int.MaxValue);
            this.Map(x => x.ValueTypeName);
            this.References(x => x.ControlSettingsParam).Column($"{nameof(ControlSettingsParamValue.ControlSettingsParam)}Id");
        }
    }
}
