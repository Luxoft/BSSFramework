﻿using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Configuration.Generated.DAL.NHibernate.Mapping;

public class ControlSettingsMap : ConfigurationBaseMap<ControlSettings>
{
    public ControlSettingsMap()
    {
        this.Map(x => x.Name).Length(int.MaxValue).Not.Nullable();
        this.Map(x => x.AccountName);
        this.Map(x => x.Type);
        this.References(x => x.Parent).Column($"{nameof(ControlSettings.Parent)}Id");
        this.HasMany(x => x.Children).AsSet().Inverse().Cascade.None();
        this.HasMany(x => x.ControlSettingsParams).AsSet().Inverse().Cascade.AllDeleteOrphan();
    }
}
