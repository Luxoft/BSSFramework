using System;

using Framework.Core;
using Framework.DomainDriven.Attributes;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Configuration.Domain
{
    [NotAuditedClass]
    /// <summary>
    /// Связь между подпиской и бизнес-ролью
    /// </summary>
    public class SubBusinessRole : AuditPersistentDomainObjectBase, IDetail<Subscription>
    {
        private Guid businessRoleId;
        private string name;
        private readonly Subscription subscription;


        protected SubBusinessRole()
        {

        }

        /// <summary>
        /// Конструктор создает бизнес-роль с ссылкой на подписку
        /// </summary>
        /// <param name="subscription">Подписка</param>
        public SubBusinessRole(Subscription subscription)
        {
            if (subscription == null) throw new ArgumentNullException(nameof(subscription));


            this.subscription = subscription;
            this.subscription.AddDetail(this);
        }

        /// <summary>
        /// Подписка бизнес-роли
        /// </summary>
        public virtual Subscription Subscription
        {
            get { return this.subscription; }
        }

        /// <summary>
        /// ID бизнес-роли
        /// </summary>
        [Required]
        [UniqueElement]
        public virtual Guid BusinessRoleId
        {
            get { return this.businessRoleId; }
            set { this.businessRoleId = value; }
        }

        /// <summary>
        /// Название бизнес-роли
        /// </summary>
        [Required]
        public virtual string Name
        {
            get { return this.name.TrimNull(); }
            internal protected set { this.name = value.TrimNull(); }
        }

        Subscription IDetail<Subscription>.Master
        {
            get { return this.subscription; }
        }
    }
}