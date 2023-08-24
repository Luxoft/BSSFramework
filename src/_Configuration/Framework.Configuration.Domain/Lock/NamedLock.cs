using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Lock;

namespace Framework.Configuration.Domain;

/// <summary>
/// Объект, с помощью которого можно реализовать пессимистическую блокировку в базе данных
/// </summary>
[BLLRole]
public class NamedLock : AuditPersistentDomainObjectBase, INamedLock<NamedLockOperation>
{
    private NamedLockOperation lockOperation;

    /// <summary>
    /// Константа для идентификации объекта, на котором можно сделать пессимистическую блокировку
    /// </summary>
    public virtual NamedLockOperation LockOperation
    {
        get { return this.lockOperation; }
        set { this.lockOperation = value; }
    }
}
