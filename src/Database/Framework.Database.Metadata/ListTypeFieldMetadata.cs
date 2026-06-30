using Anch.Core;

using Framework.Core;
using Framework.Database.Mapping;
using Framework.Relations;

namespace Framework.Database.Metadata;

public class ListTypeFieldMetadata(
    Type domainType,
    string name,
    bool isAutoGenerateField,
    Type type,
    IEnumerable<Attribute> attributes,
    DomainTypeMetadata domainTypeMetadata)
    : FieldMetadata(name, type, attributes, domainTypeMetadata)
{
    private readonly Type _domainType = domainType ?? throw new ArgumentNullException(nameof(domainType));

    readonly Type elementType = type.GetGenericArguments()[0];
    public Type ElementType => this.elementType;

    public bool IsVirtual => !this._domainType.GetProperty(this.Name.ToStartUpperCase()).Maybe(prop => prop.PropertyType == this.Type);

    public CascadeMode CascadeMode
    {
        get
        {
            var attributes = this.Attributes.OfType<MappingAttribute>().Where(attribute => attribute.CascadeMode != CascadeMode.Auto).ToList();

            return attributes.Any() ? (attributes.First().GetActualCascadeMode() ?? CascadeMode.Auto) : CascadeMode.Auto;
        }
    }

    public bool Immutable
    {
        get
        {
            var attributes = this.Attributes.OfType<DetailRoleAttribute>().ToList();

            if (attributes.Count > 1)
            {
                throw new ArgumentException("Declare more then one set immutable identifier");
            }

            if (!attributes.Any())
            {
                return false;
            }
            return attributes.First().HasValue(false);
        }
    }

    public bool IsCompilerGenerated => isAutoGenerateField;
}

