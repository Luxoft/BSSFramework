using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.Generation.Domain;
using Framework.Projection;

using JetBrains.Annotations;

namespace Framework.DomainDriven.ProjectionGenerator;

internal static class DomainMetadataExtensions
{
    public static bool HasCustomProjectionProperties([NotNull] this IDomainMetadata environment, [NotNull] Type domainType)
    {
        if (environment == null) throw new ArgumentNullException(nameof(environment));
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        return environment.GetProjectionProperties(domainType, false, true).Any();
    }

    public static IEnumerable<PropertyInfo> GetProjectionProperties([NotNull] this IDomainMetadata environment, [NotNull] Type domainType, bool includeBase, bool? customPropFilter)
    {
        if (environment == null) { throw new ArgumentNullException(nameof(environment)); }
        if (domainType == null) { throw new ArgumentNullException(nameof(domainType)); }

        foreach (var property in domainType.GetProperties(BindingFlags.Instance | BindingFlags.Public).OrderBy(property => property.Name))
        {
            var isCustom = property.HasAttribute<ProjectionPropertyAttribute>(attr => attr.Role == ProjectionPropertyRole.Custom);

            if (customPropFilter == null || isCustom == customPropFilter)
            {
                var isBaseProp = property.ReflectedType.IsAssignableFrom(domainType.BaseType);//.IsAssignableFrom() environment.IsDomainObjectBaseProperty(property) || environment.IsPersistentDomainObjectBaseProperty(property);

                if (includeBase || !isBaseProp)
                {
                    yield return property;
                }
            }
        }
    }
}
