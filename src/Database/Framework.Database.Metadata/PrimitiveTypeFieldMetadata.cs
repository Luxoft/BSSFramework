namespace Framework.Database.Metadata;

public class PrimitiveTypeFieldMetadata(
    string name,
    Type type,
    IEnumerable<Attribute> attributes,
    DomainTypeMetadata domainTypeMetadata,
    bool isIdentity,
    bool isCollection = false)
    : FieldMetadata(name, type, attributes, domainTypeMetadata)
{
    public bool IsCollection => isCollection;

    public bool IsIdentity => isIdentity;
}
