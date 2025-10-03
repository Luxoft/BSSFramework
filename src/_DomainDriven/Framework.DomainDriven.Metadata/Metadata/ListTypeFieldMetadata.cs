using CommonFramework;

using Framework.Core;
using Framework.Persistent;
using Framework.Persistent.Mapping;

namespace Framework.DomainDriven.Metadata;

public class ListTypeFieldMetadata : FieldMetadata
{
    private readonly Type _domainType;

    private readonly bool _isCompilerGenerated;
    readonly Type elementType;
    public Type ElementType
    {
        get { return this.elementType; }
    }

    public ListTypeFieldMetadata(Type domainType,
                                 string name,
                                 bool isAutoGenerateField,
                                 Type type,
                                 IEnumerable<Attribute> attributes,
                                 DomainTypeMetadata domainTypeMetadata)
            : base(name, type, attributes, domainTypeMetadata)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        this._domainType = domainType;
        this._isCompilerGenerated = isAutoGenerateField;
        this.elementType = type.GetGenericArguments()[0];
    }


    public bool IsVirtual
    {
        get
        {
            return !this._domainType.GetProperty(this.Name.ToStartUpperCase()).Maybe(prop => prop.PropertyType == this.Type);
        }
    }

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
                throw new ArgumentException("Declare more then one set immuble indentifier");
            }

            if (!attributes.Any())
            {
                return false;
            }
            return attributes.First().HasValue(false);
        }
    }

    public bool IsCompilerGenerated
    {
        get { return this._isCompilerGenerated; }
    }
}
