using System.Reflection;

namespace Framework.FileGeneration.Configuration;

public static class FileGeneratorConfigurationExtensions
{
    extension(IFileGeneratorConfiguration<IFileGenerationEnvironment> configuration)
    {
        public bool IsDomainObject(Type type) => !type.IsAbstract && configuration.Environment.DomainObjectBaseType.IsAssignableFrom(type);

        public bool IsPersistentObject(Type type)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (type == null) throw new ArgumentNullException(nameof(type));

            return !type.IsAbstract && configuration.Environment.PersistentDomainObjectBaseType.IsAssignableFrom(type);
        }

        public bool IsIdentityProperty(PropertyInfo property)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (property == null) throw new ArgumentNullException(nameof(property));

            return configuration.Environment.IsIdentityProperty(property);
        }
    }
}
