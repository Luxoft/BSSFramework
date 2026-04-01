using Framework.Database.Mapping;
using Framework.Relations;

namespace Framework.Database.Metadata;

public class ReferenceTypeFieldMetadata(string name, Type type, IEnumerable<Attribute> attributes, DomainTypeMetadata domainTypeMetadata)
    : FieldMetadata(name, type, attributes, domainTypeMetadata)
{
    public Type ToType => this.Type;

    public Type FromType => this.DomainTypeMetadata.DomainType;

    public bool IsMasterReference => this.Attributes.OfType<IsMasterAttribute>().Any();

    public bool IsOneToOneReference => this.Attributes.OfType<MappingAttribute>().Any(attribute => attribute.IsOneToOne);

    public CascadeMode CascadeMode
    {
        get
        {
            var attributes = this.Attributes.OfType<MappingAttribute>().Where(attribute => attribute.CascadeMode != CascadeMode.Auto).ToList();

            return attributes.Any() ? (attributes.First().GetActualCascadeMode() ?? CascadeMode.Disabled) : CascadeMode.Disabled;
        }
    }

}
