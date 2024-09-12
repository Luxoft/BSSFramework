using System.Reflection;

namespace Framework.DomainDriven.NHibernate;

public static class MappingSettingsExtensions
{
    public static MappingSettings AddInitializer(this MappingSettings mappingSettings, IConfigurationInitializer initializer)
    {
        return mappingSettings with { Initializer = mappingSettings.Initializer.Add(initializer) };
    }

    public static bool IsAudited(this MappingSettings mappingSettings)
    {
        if (mappingSettings == null) throw new ArgumentNullException(nameof(mappingSettings));

        return mappingSettings.AuditDatabase != null;
    }

    public static IEnumerable<Assembly> GetDomainTypeAssemblies(this MappingSettings mappingSettings)
    {
        if (mappingSettings == null) throw new ArgumentNullException(nameof(mappingSettings));

        return mappingSettings.Types.Select(t => t.Assembly).Distinct();
    }

    public static bool IsAuditInMainDatabase(this MappingSettings mappingSettings)
    {
        if (mappingSettings == null) throw new ArgumentNullException(nameof(mappingSettings));

        return string.Equals(mappingSettings.Database.Name, mappingSettings.AuditDatabase?.Name);
    }
}
