namespace Framework.DomainDriven.Metadata;

public class AssemblyMetadata : IAssemblyMetadata
{
    public AssemblyMetadata(Type persistentDomainObjectBaseType)
    {
        if (persistentDomainObjectBaseType == null) throw new ArgumentNullException(nameof(persistentDomainObjectBaseType));

        this.PersistentDomainObjectBaseType = persistentDomainObjectBaseType;
    }


    public Type PersistentDomainObjectBaseType { get; }

    public IEnumerable<DomainTypeMetadata> DomainTypes { get; set; }
}
