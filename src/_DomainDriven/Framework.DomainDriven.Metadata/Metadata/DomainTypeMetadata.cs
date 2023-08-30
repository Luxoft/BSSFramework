using Framework.Core;
using Framework.Persistent.Mapping;

namespace Framework.DomainDriven.Metadata;

public class DomainTypeMetadata
{
    private readonly Type type;
    private readonly AssemblyMetadata assemblyMetadata;
    private readonly List<FieldMetadata> fields;
    private readonly List<DomainTypeMetadata> children;
    private readonly Lazy<IList<UniqueIndexMetadata>> indexUniqueMetadataLazy;
    private bool isCollected;
    private DomainTypeMetadata parent;

    public DomainTypeMetadata(Type domainType, AssemblyMetadata assemblyMetadata)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        this.type = domainType;
        this.assemblyMetadata = assemblyMetadata;
        this.fields = new List<FieldMetadata>();
        this.children = new List<DomainTypeMetadata>();

        this.indexUniqueMetadataLazy = new Lazy<IList<UniqueIndexMetadata>>(this.CreateUniqueIndexMetadata);
    }

    public DomainTypeMetadata Parent => this.parent;

    public AssemblyMetadata AssemblyMetadata => this.assemblyMetadata;

    public Type DomainType => this.type;

    public bool IsView => this.DomainType.HasAttribute<ViewAttribute>();

    public bool IsInlineBaseType => this.DomainType.HasAttribute<InlineBaseTypeMappingAttribute>();

    public bool UseSmartUpdate => this.DomainType.HasAttribute<SmartUpdateAttribute>();

    public IEnumerable<FieldMetadata> Fields => this.fields;

    public IEnumerable<ReferenceTypeFieldMetadata> ReferenceFields => this.Fields.OfType<ReferenceTypeFieldMetadata>();

    public IEnumerable<ListTypeFieldMetadata> ListFields => this.Fields.OfType<ListTypeFieldMetadata>();

    public IEnumerable<PrimitiveTypeFieldMetadata> PrimitiveFields => this.Fields.OfType<PrimitiveTypeFieldMetadata>();

    public IEnumerable<InlineTypeFieldMetadata> InlineFields => this.Fields.OfType<InlineTypeFieldMetadata>();

    public IEnumerable<DomainTypeMetadata> NotAbstractChildrenDomainTypes
    {
        get => this.children.Where(z => !z.DomainType.IsAbstract);
    }

    public DomainTypeMetadata Root
    {
        get => this.GetAllElements(v => v.Parent).Last();
    }

    public IEnumerable<UniqueIndexMetadata> UniqueIndexes => this.indexUniqueMetadataLazy.Value;

    public IEnumerable<FieldMetadata> GetExpandedUpFields() => this.GetAllElements(z => z.Parent).SelectMany(z => z.Fields);

    public void RemoveField(FieldMetadata fieldMetadata)
    {
        this.ValidateChangeState();
        this.fields.Remove(fieldMetadata);
    }

    public void AddFields(IEnumerable<FieldMetadata> fieldMetadatas)
    {
        this.ValidateChangeState();

        this.fields.AddRange(fieldMetadatas);
    }

    public void EndDeclaration()
    {
        this.isCollected = true;
    }

    public override string ToString() => this.type.Name;

    internal void AddChildren(DomainTypeMetadata child)
    {
        this.ValidateChangeState();

        child.parent = this;
        this.children.Add(child);
    }

    private void ValidateChangeState()
    {
        if (this.isCollected)
        {
            throw new ArgumentException($"{this.GetType().Name} has initialed state. Change object in this state impossible");
        }
    }

    private IList<UniqueIndexMetadata> CreateUniqueIndexMetadata()
    {
        if (!this.isCollected)
        {
            throw new ArgumentException($"{this.GetType().Name} has initializing state. Use EndDeclaration method for complete object state");
        }

        var result = new UniqueIndexMetadataReader(this.assemblyMetadata).Read(this).ToList();

        var grouped = result.GroupBy(z => z.Name).Select(z => new { Key = z.Key, UniqueIndexMetadataCollection = z.Distinct().ToList() }).ToList();
        if (grouped.Any(z => z.UniqueIndexMetadataCollection.Count > 1))
        {
            throw new ArgumentException(
                                        $"UniqueIndexSet has more then one names:'{result.GroupBy(z => z.Name).Where(z => z.Skip(1).Any()).Select(z => z.Key).Join(",")}' for '{this.DomainType.Name}' domain type");
        }

        return grouped.Select(z => z.UniqueIndexMetadataCollection.First()).ToList();
    }
}
