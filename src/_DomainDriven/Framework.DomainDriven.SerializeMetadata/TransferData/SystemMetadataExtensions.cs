using System;
using System.Linq;

using Framework.Core;
using Framework.OData;
using Framework.Persistent;

namespace Framework.DomainDriven.SerializeMetadata;

public static class SystemMetadataExtensions
{
    public static DomainTypeSubsetMetadata GetDomainTypeMetadataFullSubset(this SystemMetadata systemMetadata, TypeHeader domainType, IDynamicSelectOperation selectOperation)
    {
        var startDomainType = systemMetadata.GetDomainType(domainType);

        var expandProperties = selectOperation.Expands.ToArray(l => l.GetPropertyPath().ToArray());

        var selectsProperties = selectOperation.Selects.ToArray(se => se.GetPropertyPath().ToArray());

        var isFullSelect = !selectsProperties.Any();

        var expandsSubset = new DomainTypePropertyExpandLoader(systemMetadata)
                .GetDomainTypeMetadataSubset(startDomainType, isFullSelect ? expandProperties : selectsProperties);

        var selectSubset =

                new DomainTypePropertySelectLoader(systemMetadata, isFullSelect)

                        .GetDomainTypeMetadataSubset(expandsSubset, isFullSelect ? expandProperties : selectsProperties);

        return selectSubset;
    }



    public static DomainTypeMetadata GetDomainType(this SystemMetadata systemMetadata, TypeHeader type)
    {
        if (systemMetadata == null) throw new ArgumentNullException(nameof(systemMetadata));
        if (type == null) throw new ArgumentNullException(nameof(type));

        return systemMetadata.Types.Where(t => t.Role.HasSubset())
                             .CastStrong<TypeMetadata, DomainTypeMetadata>()
                             .GetById(type);
    }
}
