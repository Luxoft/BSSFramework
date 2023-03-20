using System.Collections.ObjectModel;
using System.Reflection;

namespace Framework.DomainDriven.Generation.Domain;

public interface IDomainMetadata : IDomainMetadataBase
{
    ReadOnlyCollection<Assembly> DomainObjectAssemblies { get; }
}
