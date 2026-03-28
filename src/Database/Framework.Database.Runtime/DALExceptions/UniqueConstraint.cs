using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.RegularExpressions;

using CommonFramework;

namespace Framework.Database.DALExceptions;

public struct UniqueConstraint
{
    private static readonly Regex FieldNameRegex = new Regex("(\\S*)Id");

    public UniqueConstraint(DomainObjectInfo domainObjectInfo, string name, IEnumerable<string> properties, IDalValidationIdentitySource validationIdentitySource)
        : this()
    {
        this.Name = name;
        this.ObjectInfo = domainObjectInfo;
        this.Properties = properties.Select(x => GetName(domainObjectInfo.Type.GetProperties(), x.Trim('[', ']'), validationIdentitySource)).ToReadOnlyCollection();
    }

    public DomainObjectInfo ObjectInfo { get; }

    public string Name { get; }

    public ReadOnlyCollection<string> Properties { get; }

    private static string GetName(ICollection<PropertyInfo> properties, string columnName, IDalValidationIdentitySource validationIdentitySource)
    {
        var property = properties.FirstOrDefault(x => string.Equals(x.Name, GetFieldName(columnName), StringComparison.InvariantCultureIgnoreCase));

        return property != null ? validationIdentitySource.GetPropertyValidationName(property) : columnName;
    }

    private static string GetFieldName(string columnName)
    {
        var match = FieldNameRegex.Match(columnName);

        return match.Success ? match.Groups[1].Value : columnName;
    }
}
