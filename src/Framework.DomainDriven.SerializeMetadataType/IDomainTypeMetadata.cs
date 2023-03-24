using System.Collections.Generic;

namespace Framework.DomainDriven.SerializeMetadata;

public interface IDomainTypeMetadata : ITypeMetadata
{
    IEnumerable<IPropertyMetadata> Properties { get; }

    bool IsHierarchical { get; }
}
