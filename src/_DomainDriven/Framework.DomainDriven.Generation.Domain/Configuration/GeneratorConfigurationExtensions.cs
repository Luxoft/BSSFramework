using System;
using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.DomainDriven.Generation.Domain;

public static class GeneratorConfigurationExtensions
{
    public static bool IsDomainObject(this IGeneratorConfiguration<IGenerationEnvironment> configuration, Type type)
    {
        return !type.IsAbstract && configuration.Environment.DomainObjectBaseType.IsAssignableFrom(type);
    }

    public static bool IsPersistentObject(this IGeneratorConfiguration<IGenerationEnvironment> configuration, Type type)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        if (type == null) throw new ArgumentNullException(nameof(type));

        return !type.IsAbstract && configuration.Environment.PersistentDomainObjectBaseType.IsAssignableFrom(type);
    }

    public static CodeTypeReference GetIdentityObjectCodeTypeReference(this IGeneratorConfiguration<IGenerationEnvironment> configuration)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));

        return typeof(IIdentityObject<>).MakeGenericType(configuration.Environment.IdentityProperty.PropertyType).ToTypeReference();
    }

    public static bool IsIdentityProperty(this IGeneratorConfiguration<IGenerationEnvironment> configuration, [NotNull] PropertyInfo property)
    {
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        if (property == null) throw new ArgumentNullException(nameof(property));

        return configuration.Environment.IsIdentityProperty(property);
    }
}
