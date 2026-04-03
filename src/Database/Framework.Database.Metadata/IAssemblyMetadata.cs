namespace Framework.Database.Metadata;

public interface IAssemblyMetadata
{
    Type PersistentDomainObjectBaseType { get; }

    IEnumerable<DomainTypeMetadata> DomainTypes { get; }
}
