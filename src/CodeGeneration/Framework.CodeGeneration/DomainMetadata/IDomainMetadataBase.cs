using System.Reflection;

namespace Framework.CodeGeneration.DomainMetadata;

public interface IDomainMetadataBase
{
    PropertyInfo IdentityProperty { get; }


    Type DomainObjectBaseType { get; }

    Type PersistentDomainObjectBaseType { get; }

    Type AuditPersistentDomainObjectBaseType { get; }
}
