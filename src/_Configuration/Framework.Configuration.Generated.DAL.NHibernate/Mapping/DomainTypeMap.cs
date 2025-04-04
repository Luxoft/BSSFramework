﻿using Framework.Configuration.Domain;
using Framework.Configuration.Generated.DAL.NHibernate.Mapping.Base;

namespace Framework.Configuration.Generated.DAL.NHibernate.Mapping;

public class DomainTypeMap : ConfigurationBaseMap<DomainType>
{
    public DomainTypeMap()
    {
        this.Map(x => x.Name)
            .UniqueKey("UIX_name_nameSpace_targetSystemDomainType")
            .Not.Nullable();
        this.Map(x => x.NameSpace)
            .UniqueKey("UIX_name_nameSpace_targetSystemDomainType");
        this.References(x => x.TargetSystem).Column($"{nameof(DomainType.TargetSystem)}Id")
            .UniqueKey("UIX_name_nameSpace_targetSystemDomainType").Not.Nullable();
        this.HasMany(x => x.EventOperations).AsSet().Inverse().Cascade.AllDeleteOrphan();
    }
}
