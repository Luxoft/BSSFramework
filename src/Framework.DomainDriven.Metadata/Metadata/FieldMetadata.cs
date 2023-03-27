using System.Reflection;

using Framework.Core;
using Framework.Persistent;
using Framework.Persistent.Mapping;

namespace Framework.DomainDriven.Metadata;

public abstract class FieldMetadata
{
    private readonly string name;
    private readonly Type type;
    private readonly IEnumerable<Attribute> attributes;
    private readonly DomainTypeMetadata domainTypeMetadata;

    protected FieldMetadata(string name, Type type, IEnumerable<Attribute> attributes, DomainTypeMetadata domainTypeMetadata)
    {
        this.name = name;
        this.domainTypeMetadata = domainTypeMetadata;
        this.type = type;
        this.attributes = attributes;
    }

    public DomainTypeMetadata DomainTypeMetadata
    {
        get { return this.domainTypeMetadata; }
    }

    public string Name
    {
        get { return this.name; }
    }
    public Type Type
    {
        get { return this.type; }
    }
    public IEnumerable<Attribute> Attributes
    {
        get { return this.attributes; }
    }
    public bool IsVersion
    {
        get
        {
            return this.Attributes.Any(q => q is VersionAttribute);
        }
    }

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
