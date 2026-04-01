using System.Reflection;

using CommonFramework;

using Framework.Core;
using Framework.Database.Attributes;
using Framework.Database.Mapping;

namespace Framework.Database.Metadata;

public abstract class FieldMetadata(string name, Type type, IEnumerable<Attribute> attributes, DomainTypeMetadata domainTypeMetadata)
{
    public DomainTypeMetadata DomainTypeMetadata => domainTypeMetadata;

    public string Name => name;

    public Type Type => type;

    public IEnumerable<Attribute> Attributes => attributes;

    public bool IsVersion => this.Attributes.Any(q => q is VersionAttribute);

    public string ExternalTableName => this.Attributes.OfType<MappingAttribute>().SingleOrDefault().Maybe(attr => attr.ExternalTableName);

    public IEnumerable<Attribute> GetExpandedAttributes(Type type)
    {
        var currentProperty = type.GetProperties()
                                  .Where(z => string.Equals(z.Name, this.Name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
        if (null == currentProperty)
        {
            return this.Attributes;
        }

        return currentProperty.GetCustomAttributes(true).Cast<Attribute>();
    }



}

public static class FieldMetadataExtensions
{
    public static PropertyInfo GetReferencedProperty(this FieldMetadata fieldMetadata)
    {
        if (fieldMetadata == null) throw new ArgumentNullException(nameof(fieldMetadata));

        return fieldMetadata.DomainTypeMetadata.DomainType.GetProperty(fieldMetadata.Name.ToStartUpperCase(),
                                                                       () => new Exception($"Property for field {fieldMetadata.Name} not found of type:{fieldMetadata.DomainTypeMetadata.DomainType.Name}"));
    }
}
