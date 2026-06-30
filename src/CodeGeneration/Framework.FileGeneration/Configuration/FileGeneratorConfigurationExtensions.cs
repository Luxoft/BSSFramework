using System.Reflection;

namespace Framework.FileGeneration.Configuration;

public static class FileGeneratorConfigurationExtensions
{
    extension(IFileGeneratorConfiguration<IFileGenerationEnvironment> configuration)
    {
        public bool IsDomainObject(Type type) => !type.IsAbstract && configuration.Environment.DomainObjectBaseType.IsAssignableFrom(type);

        public bool IsPersistentObject(Type? type)
        {
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));

            return type is not null && !type.IsAbstract && configuration.Environment.PersistentDomainObjectBaseType.IsAssignableFrom(type);
        }

        public bool IsIdentityProperty(PropertyInfo property)
        {
            if (configuration is null) throw new ArgumentNullException(nameof(configuration));
            if (property is null) throw new ArgumentNullException(nameof(property));

            return configuration.Environment.IsIdentityProperty(property);
        }
    }
}

