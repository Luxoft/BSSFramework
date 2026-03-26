using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.Generation.Domain;
using Framework.Projection;

namespace Framework.DomainDriven.ProjectionGenerator;

internal static class DomainMetadataExtensions
{
    public static bool HasCustomProjectionProperties(this IDomainMetadata environment, Type domainType)
    {
        if (environment == null) throw new ArgumentNullException(nameof(environment));
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        return environment.GetProjectionProperties(domainType, false, true).Any();
    }

    public static IEnumerable<PropertyInfo> GetProjectionProperties(this IDomainMetadata environment, Type domainType, bool includeBase, bool? customPropFilter)
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
