using System;

using Framework.DomainDriven.Attributes;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Configuration.Domain
{
    /// <summary>
    /// Элемент типизированного контекста
    /// </summary>
    [NotAuditedClass]
    public class SubscriptionSecurityItem : AuditPersistentDomainObjectBase,
        IDetail<Subscription>,
        ISubscriptionElement,
        IDomainTypeElement<DomainType>,
        IAuthDomainTypeElement
    {
        private readonly Subscription subscription;

        private NotificationExpandType expandType;

        private SubscriptionLambda source;

        private Guid authDomainTypeId;


        protected SubscriptionSecurityItem()
        {

        }

        /// <summary>
        /// Конструктор создает элемент типизированного контекста с ссылкой на подписку
        /// </summary>
        /// <param name="subscription"></param>
        public SubscriptionSecurityItem(Subscription subscription)
        {
            if (subscription == null) throw new ArgumentNullException(nameof(subscription));

            this.subscription = subscription;
            this.subscription.AddDetail(this);
        }

        /// <summary>
        /// Подписка для типизированного контекста
        /// </summary>
        public virtual Subscription Subscription
        {
            get { return this.subscription; }
        }

        /// <summary>
        /// Лямбла, получающая типизированный контекст для ролей подписки
        /// </summary>
        /// <remarks>
        /// Лямбда подписки типа "AuthSource"
        /// </remarks>
        [Required]
        [DomainTypeValidator]
        [SubscriptionLambdaTypeValidator(SubscriptionLambdaType.AuthSource)]
        public virtual SubscriptionLambda Source
        {
            get { return this.source; }
            set { this.source = value; }
        }

        /// <summary>
        /// Тип Expand Type, отображающий расширение прав по дереву
        /// </summary>
        public virtual NotificationExpandType ExpandType
        {
            get { return this.expandType; }
            set { this.expandType = value; }
        }

        /// <summary>
        /// Доменный тип типизированного контекста
        /// </summary>
        [UniqueElement]
        [SyncAuthDomainTypeValidator]
        [Required]
        public virtual Guid AuthDomainTypeId
        {
            get { return this.authDomainTypeId; }
            set { this.authDomainTypeId = value; }
        }

        Subscription IDetail<Subscription>.Master
        {
            get { return this.Subscription; }
        }

        [ExpandPath("Subscription.DomainType")]
        DomainType IDomainTypeElement<DomainType>.DomainType
        {
            get { return this.Subscription.DomainType; }
        }
    }
}