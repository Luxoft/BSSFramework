using System;

namespace Framework.DomainDriven.BLL;

public interface IPersistentDomainObjectBaseTypeContainer
{
    Type PersistentDomainObjectBaseType { get; }
}
