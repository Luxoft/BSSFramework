namespace Framework.DomainDriven.Metadata;

public interface IAssemblyMetadata
{
    Type PersistentDomainObjectBaseType { get; }

    IEnumerable<DomainTypeMetadata> DomainTypes { get; }
}
