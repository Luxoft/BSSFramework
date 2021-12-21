using System;
using System.Linq;
using System.Reflection;

using Framework.Core;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;

namespace Framework.DomainDriven.DTOGenerator.Server
{
    public static class Extensions
    {
        public static PropertyInfo TryGetIntegrationVersionProperty(this Type domainType)
        {
            var integrationVersionProperty = domainType
                                             .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                             .Where(z => z.HasAttribute<IntegrationVersionAttribute>())
                                             .SingleOrDefault((properties) => new ArgumentException($"Type:{domainType} has more then one property with {nameof(IntegrationVersionAttribute)} attribute. FindedProperties:{properties.Select(z => z.Name).Join(",")}"));

            if (null != integrationVersionProperty && integrationVersionProperty.IsIgnored(DTORole.Integration))
            {
                throw new ArgumentException(
                    $"Type:{domainType} has property:{integrationVersionProperty.Name} with attribute:{nameof(IntegrationVersionAttribute)} but mark as ignore Serialization for {DTORole.Integration}");
            }

            return integrationVersionProperty;
        }

        public static bool IsIntegrationVersion(this Type domainType) => domainType.TryGetIntegrationVersionProperty() != null;
    }
}
