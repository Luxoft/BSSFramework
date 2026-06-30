namespace Framework.Database.Metadata;

public class AssemblyMetadata(Type persistentDomainObjectBaseType) : IAssemblyMetadata
{
    public Type PersistentDomainObjectBaseType { get; } = persistentDomainObjectBaseType ?? throw new ArgumentNullException(nameof(persistentDomainObjectBaseType));

    public IEnumerable<DomainTypeMetadata> DomainTypes { get; set; } = null!;
}
