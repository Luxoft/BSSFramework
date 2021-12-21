using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security.Lock;
using Framework.Restriction;

namespace Framework.Workflow.Domain
{
    /// <summary>
    /// Объект, с помощью которого можно реализовать пессимистическую блокировку в базе данных
    /// </summary>
    [BLLRole]
    [UniqueGroup]
    public class NamedLock : AuditPersistentDomainObjectBase, INamedLock<NamedLockOperation>
    {
        private NamedLockOperation lockOperation;

        /// <summary>
        /// Константа для идентификации объекта, на котором можно сделать пессимистическую блокировку
        /// </summary>
        [UniqueElement]
        public virtual NamedLockOperation LockOperation
        {
            get { return this.lockOperation; }
            set { this.lockOperation = value; }
        }
    }
}
