using System.Collections.ObjectModel;
using System.Reflection;

namespace Framework.FileGeneration.Configuration;

public interface IDomainMetadata
{
    ReadOnlyCollection<Assembly> DomainObjectAssemblies { get; }

    PropertyInfo IdentityProperty { get; }

    Type DomainObjectBaseType { get; }

    Type PersistentDomainObjectBaseType { get; }

    Type AuditPersistentDomainObjectBaseType { get; }
}
