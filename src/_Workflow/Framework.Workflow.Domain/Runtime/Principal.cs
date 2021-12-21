using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Workflow.Domain.Runtime
{

    /// <summary>
    /// Объект для описания и идентификации пользователя системы в воркфлоу
    /// </summary>
    public abstract class Principal : AuditPersistentDomainObjectBase
    {
        private string login;

        /// <summary>
        /// Логин сотрудника
        /// </summary>
        [Required]
        [UniqueElement]
        public virtual string Login
        {
            get { return this.login; }
            set { this.SetValueSafe(v => v.login, value); }
        }
    }
}