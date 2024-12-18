using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Configuration.Generated.DAL.NHibernate.Mapping;

public class SystemConstantMap : ConfigurationBaseMap<SystemConstant>
{
    public SystemConstantMap()
    {
        this.Map(x => x.Code).UniqueKey("UIX_codeSystemConstant").Not.Nullable();
        this.Map(x => x.Description).Length(int.MaxValue);
        this.Map(x => x.Value).Length(int.MaxValue);
        this.Map(x => x.IsManual);
        this.References(x => x.Type).Column($"{nameof(SystemConstant.Type)}Id").Not.Nullable();
    }
}
