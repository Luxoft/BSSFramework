using System;
using System.Reflection;

namespace Framework.DomainDriven.Generation.Domain;

public interface IDomainMetadataBase
{
    PropertyInfo IdentityProperty { get; }


    Type DomainObjectBaseType { get; }

    Type PersistentDomainObjectBaseType { get; }

    Type AuditPersistentDomainObjectBaseType { get; }
}
