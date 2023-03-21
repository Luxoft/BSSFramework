using Framework.DomainDriven.BLL.Security.Lock;

namespace Framework.Configuration.Domain;

/// <summary>
/// Константы для идентификации объекта, на котором можно сделать пессимистическую блокировку
/// </summary>
public enum NamedLockOperation
{
    [GlobalLock(typeof(DomainType))]
    UpdateDomainTypeLock,

    [GlobalLock(typeof(Sequence))]
    UpdateSequence,

    [GlobalLock(typeof(DomainObjectModification))]
    ProcessModifications
}
