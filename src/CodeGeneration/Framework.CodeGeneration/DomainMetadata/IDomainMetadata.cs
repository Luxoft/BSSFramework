using System.Collections.ObjectModel;
using System.Reflection;

namespace Framework.CodeGeneration.DomainMetadata;

public interface IDomainMetadata : IDomainMetadataBase
{
    ReadOnlyCollection<Assembly> DomainObjectAssemblies { get; }
}
