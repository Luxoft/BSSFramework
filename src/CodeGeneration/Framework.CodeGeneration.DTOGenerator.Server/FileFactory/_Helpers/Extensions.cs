using System.Reflection;

using CommonFramework;

using Framework.BLL.Domain.Attributes;
using Framework.BLL.Domain.Serialization;
using Framework.BLL.Domain.Serialization.Extensions;
using Framework.Core;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileFactory._Helpers;

public static class Extensions
{
    public static PropertyInfo? TryGetIntegrationVersionProperty(this Type domainType)
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
