using System;

using Framework.DomainDriven.Attributes;
using Framework.Persistent;
using Framework.Core;
using Framework.Restriction;
using Framework.DomainDriven.BLL;

namespace Framework.Configuration.Domain
{
    /// <summary>
    /// Описание доменного объекта, над которым производятся высокоуровневые действия
    /// </summary>
    [BLLViewRole]
    [NotAuditedClass]
    [Obsolete("v10.0 Кандидат на вылет в следующих версиях")]
    [ConfigurationViewDomainObject(ConfigurationSecurityOperationCode.Disabled)]
    public class UserActionObject : AuditPersistentDomainObjectBase, IDetail<UserAction>, IVisualIdentityObject
    {
        private Guid objectIdentity;

        private string name;

        private readonly UserAction userAction;

        /// <summary>
        /// Конструктор создает доменный объектом с ссылкой на User Action
        /// </summary>
        /// <param name="userAction"></param>
        public UserActionObject(UserAction userAction)
        {
            if (userAction == null)
            {
                throw new ArgumentNullException(nameof(userAction));
            }

            this.userAction = userAction;
            this.userAction.AddUserActionObject(this);
        }

        protected UserActionObject()
        {
        }

        /// <summary>
        /// Высокоуровневое действие пользователя в системе
        /// </summary>
        [Required]
        public virtual UserAction UserAction
        {
            get
            {
                return this.userAction;
            }
        }

        /// <summary>
        /// ID объекта, над которым были произведены действия
        /// </summary>
        public virtual Guid ObjectIdentity
        {
            get
            {
                return this.objectIdentity;
            }

            set
            {
                this.objectIdentity = value;
            }
        }

        /// <summary>
        /// Имя высокоуровневой операции
        /// </summary>
        public virtual string Name
        {
            get
            {
                return this.name.TrimNull();
            }

            set
            {
                this.name = value.TrimNull();
            }
        }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        [ExpandPath("UserAction.Name")]
        public virtual string UserActionName => this.UserAction.Name;

        /// <summary>
        /// Имя доменного типа, к которому принаджелит высокоуровневая операция
        /// </summary>
        [ExpandPath("UserAction.DomainType.Name")]
        public virtual string DomainTypeName => this.UserAction.DomainType.Name;

        UserAction IDetail<UserAction>.Master => this.UserAction;
    }
}
