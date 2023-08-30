using System.Reflection;

namespace Framework.DomainDriven.NHibernate;

public static class MappingSettingsExtensions
{
    public static bool IsAudited(this IMappingSettings mappingSettings)
    {
        if (mappingSettings == null) throw new ArgumentNullException(nameof(mappingSettings));

        return mappingSettings.AuditDatabase != null;
    }

    public static IEnumerable<Assembly> GetDomainTypeAssemblies(this IMappingSettings mappingSettings)
    {
        if (mappingSettings == null) throw new ArgumentNullException(nameof(mappingSettings));

        return mappingSettings.Types.Select(t => t.Assembly).Distinct();
    }

    public static bool IsAuditInMainDatabase(this IMappingSettings mappingSettings)
    {
        if (mappingSettings == null) throw new ArgumentNullException(nameof(mappingSettings));

        return string.Equals(mappingSettings.Database?.Name, mappingSettings.AuditDatabase?.Name);
    }
}
