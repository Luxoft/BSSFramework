using System;

namespace Framework.DomainDriven;

public interface IPersistentDomainObjectBaseTypeContainer
{
    Type PersistentDomainObjectBaseType { get; }
}
