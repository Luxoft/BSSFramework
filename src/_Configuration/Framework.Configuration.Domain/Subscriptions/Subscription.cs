using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

using Framework.Core;
using Framework.DomainDriven.Attributes;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Notification;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Restriction;
using Framework.Validation;

namespace Framework.Configuration.Domain
{
    /// <summary>
    /// Подписка, включающая в себя совокупность обязательных объектов/условий для отправки нотификаций
    /// </summary>
    [RequiredGroupValidator(RequiredGroupValidatorMode.AllOrNothing, GroupKey = "DynamicSourceMode")]
    [RequiredGroupValidator(RequiredGroupValidatorMode.AllOrNothing, GroupKey = "Sender")]
    [UniqueGroup]
    [ConfigurationViewDomainObject(ConfigurationSecurityOperationCode.SubscriptionView)]
    [ConfigurationEditDomainObject(ConfigurationSecurityOperationCode.SubscriptionEdit)]
    [BLLViewRole, BLLSaveRole, BLLRemoveRole]
    [NotAuditedClass]
    public class Subscription : AuditPersistentDomainObjectBase,
        IMaster<SubBusinessRole>,
        IMaster<SubscriptionSecurityItem>,
        ISubscription,
        ICodeObject,
        ITargetSystemElement<TargetSystem>,
        IDomainTypeElement<DomainType>,
        ISubscriptionElement
    {
        private readonly ICollection<SubBusinessRole> subBusinessRoles = new List<SubBusinessRole>();

        private readonly ICollection<SubscriptionSecurityItem> securityItems = new List<SubscriptionSecurityItem>();

        private DomainType domainType;

        private SubscriptionLambda condition;

        private MessageTemplate messageTemplate;

        private SubscriptionLambda generation;

        private SubscriptionLambda copyGeneration;

        private SubscriptionLambda replyToGeneration;

        private SubscriptionLambda dynamicSource;

        private SubscriptionLambda attachment;

        private string code;

        private bool excludeCurrentUser;

        private bool sendIndividualLetters;

        private bool includeAttachments = true;

        private string senderName;

        private string senderEmail;

        private NotificationExpandType? dynamicSourceExpandType;

        private RecepientsSelectorMode recepientsMode;

        private bool allowEmptyListOfRecipients;

        [NotPersistentField]
        private Type razorMessageTemplateType;

        [NotPersistentField]
        private Type metadataSourceType;

        /// <summary>
        /// Конструктор
        /// </summary>
        public Subscription()
        {
        }

        /// <summary>
        /// Коллекция дочерних ролей
        /// </summary>
        [UniqueGroup]
        public virtual IEnumerable<SubBusinessRole> SubBusinessRoles
        {
            get { return this.subBusinessRoles; }
        }

        /// <summary>
        /// Коллекция элементов секьюрного контекста
        /// </summary>
        [UniqueGroup]
        public virtual IEnumerable<SubscriptionSecurityItem> SecurityItems
        {
            get { return this.securityItems; }
        }

        public virtual DomainType DomainType
        {
            get { return this.domainType; }
            set { this.domainType = value; }
        }

        /// <summary>
        /// Целевая система, к которой относится подписка подпсики
        /// </summary>
        [ExpandPath("DomainType.TargetSystem")]
        public virtual TargetSystem TargetSystem
        {
            get { return this.DomainType.Maybe(v => v.TargetSystem); }
        }

        /// <summary>
        /// Условие подписки
        /// </summary>
        /// <remarks>
        /// Лямбда подписки типа "Condition"
        /// </remarks>
        [DomainTypeValidator]
        [SubscriptionLambdaTypeValidator(SubscriptionLambdaType.Condition)]
        [RevisionTargetSystemRequiredValidator]
        public virtual SubscriptionLambda Condition
        {
            get { return this.condition; }
            set { this.condition = value; }
        }

        /// <summary>
        /// Шаблон сообщения подписки, отправляемого пользователю в нотификации
        /// </summary>
        [Required]
        [DomainTypeValidator]
        public virtual MessageTemplate MessageTemplate
        {
            get { return this.messageTemplate; }
            set { this.messageTemplate = value; }
        }

        /// <summary>
        /// Лямбла, получающая список получателей из доменного объекта
        /// </summary>
        /// <remarks>
        /// Лямбда подписки типа "Generation"
        /// </remarks>
        [DomainTypeValidator]
        [SubscriptionLambdaTypeValidator(SubscriptionLambdaType.Generation)]
        public virtual SubscriptionLambda Generation
        {
            get { return this.generation; }
            set { this.generation = value; }
        }

        /// <summary>
        /// Лямбда, получающая список CC получателей из доменного объекта
        /// </summary>
        /// <remarks>
        /// Лямбда подписки типа "Generation"
        /// </remarks>
        [DomainTypeValidator]
        [SubscriptionLambdaTypeValidator(SubscriptionLambdaType.Generation)]
        public virtual SubscriptionLambda CopyGeneration
        {
            get { return this.copyGeneration; }
            set { this.copyGeneration = value; }
        }

        /// <summary>
        /// Лямбда, получающая список ReplyTo получателей из доменного объекта
        /// </summary>
        /// <remarks>
        /// Лямбда подписки типа "Generation"
        /// </remarks>
        [DomainTypeValidator]
        [SubscriptionLambdaTypeValidator(SubscriptionLambdaType.Generation)]
        public virtual SubscriptionLambda ReplyToGeneration
        {
            get { return this.replyToGeneration; }
            set { this.replyToGeneration = value; }
        }


        /// <summary>
        /// Лямбда, получающая список аттачей из доменного объекта.
        /// </summary>
        /// <remarks>
        /// Лямбда подписки типа "Attachment" не для хранения в базе данных.
        /// Используется только для CodeFirst подписок
        /// </remarks>
        [DomainTypeValidator]
        [CustomSerialization(CustomSerializationMode.Ignore)]
        [SubscriptionLambdaTypeValidator(SubscriptionLambdaType.Attachment)]
        public virtual SubscriptionLambda Attachment
        {
            get { return this.attachment; }
            set { this.attachment = value; }
        }

        /// <summary>
        /// Лямбла, получающая список нетипизированных контекстов для ролей подписки
        /// </summary>
        /// <remarks>
        /// Лямбда подписки типа "DynamicSource"
        /// </remarks>
        [UniqueElement("DynamicSourceMode")]
        [DomainTypeValidator]
        [SubscriptionLambdaTypeValidator(SubscriptionLambdaType.DynamicSource)]
        public virtual SubscriptionLambda DynamicSource
        {
            get { return this.dynamicSource; }
            set { this.dynamicSource = value; }
        }

        /// <summary>
        /// Тип Expand Type, отображающий расширение прав по дереву
        /// </summary>
        [UniqueElement("DynamicSourceMode")]
        [Mapping(ColumnName = "principalForRoleExpandType")]
        public virtual NotificationExpandType? DynamicSourceExpandType
        {
            get { return this.dynamicSourceExpandType; }
            set { this.dynamicSourceExpandType = value; }
        }

        /// <summary>
        /// Уникальное имя подписки
        /// </summary>
        [UniqueElement]
        [Required]
        [MaxLength(500)]
        [VisualIdentity]
        public virtual string Code
        {
            get { return this.code.TrimNull(); }
            set { this.code = value.TrimNull(); }
        }

        /// <summary>
        /// Признак того, что механизм подписки включен
        /// </summary>
        [CustomSerialization(CustomSerializationMode.Normal)]
        public override bool Active
        {
            get { return base.Active; }
            set { base.Active = value; }
        }

        /// <summary>
        /// Признак того, что нотификацию нужно отправлять с пустым списком получателей (например, в случае тестирования)
        /// </summary>
        public virtual bool AllowEmptyListOfRecipients
        {
            get { return this.allowEmptyListOfRecipients; }
            set { this.allowEmptyListOfRecipients = value; }
        }

        /// <summary>
        /// Вычисляемое свойство результатов состояний работы с контекстами
        /// </summary>
        [CustomSerialization(CustomSerializationMode.Ignore)]
        [Required, RestrictionExtension(typeof(RequiredAttribute), CustomError = "Invalid source mode")]
        public virtual SubscriptionSourceMode SourceMode
        {
            get
            {
                var isDynamicSourceMode = this.DynamicSource != null && this.DynamicSourceExpandType != null;
                var isTypeSourceMode = this.SecurityItems.Any();

                if (isDynamicSourceMode && isTypeSourceMode)
                {
                    return SubscriptionSourceMode.Invalid;
                }
                else if (isDynamicSourceMode)
                {
                    return SubscriptionSourceMode.Dynamic;
                }
                else if (isTypeSourceMode)
                {
                    return SubscriptionSourceMode.Typed;
                }
                else
                {
                    return SubscriptionSourceMode.NonContext;
                }
            }
        }


        /// <summary>
        /// Электроннный адрес отправителя
        /// </summary>
        [MaxLength(500)]
        [UniqueElement("Sender")]
        public virtual string SenderEmail
        {
            get { return this.senderEmail.TrimNull(); }
            set { this.senderEmail = value.TrimNull(); }
        }

        /// <summary>
        /// Имя отправителя
        /// </summary>
        [MaxLength(500)]
        [UniqueElement("Sender")]
        public virtual string SenderName
        {
            get { return this.senderName.TrimNull(); }
            set { this.senderName = value.TrimNull(); }
        }

        /// <summary>
        /// Признак того, что в письмо будут добавлены аттачменты из шаблона
        /// </summary>
        public virtual bool IncludeAttachments
        {
            get { return this.includeAttachments; }
            set { this.includeAttachments = value; }
        }

        /// <summary>
        /// Признак исключения пользователя из текущей рассылки
        /// </summary>
        /// <remarks>
        /// Требует специальной обработки в доменном типе. Доменный тип должен имплементировать интерфейс <see cref="ICurrentUserEmailContainer"/>
        /// </remarks>
        public virtual bool ExcludeCurrentUser
        {
            get { return this.excludeCurrentUser; }
            set { this.excludeCurrentUser = value; }
        }

        /// <summary>
        /// Признак отправки адресату(ам) индивидуального (true) или консолидированного (false) письма
        /// </summary>
        public virtual bool SendIndividualLetters
        {
            get { return this.sendIndividualLetters; }
            set { this.sendIndividualLetters = value; }
        }

        /// <summary>
        /// Cпособ комбинации адресатов рассылки по SubBusinessRoles и Generation
        /// </summary>
        public virtual RecepientsSelectorMode RecepientsMode
        {
            get { return this.recepientsMode; }
            set { this.recepientsMode = value; }
        }

        /// <summary>
        /// Задаёт или возвращает тип, реализующий Razor разметку шаблона сообщения.
        /// </summary>
        /// <value>
        /// Тип, реализующий Razor разметку шаблона сообщения.
        /// </value>
        [CustomSerialization(CustomSerializationMode.Ignore)]
        [PropertyValidationMode(false)]
        public virtual Type RazorMessageTemplateType
        {
            get { return this.razorMessageTemplateType; }
            set { this.razorMessageTemplateType = value; }
        }

        /// <summary>
        /// Задаёт или возвращает тип исходной модели подписки.
        /// </summary>
        /// <value>
        /// Тип исходной модели подписки.
        /// </value>
        [CustomSerialization(CustomSerializationMode.Ignore)]
        [PropertyValidationMode(false)]
        public virtual Type MetadataSourceType
        {
            get { return this.metadataSourceType; }
            set { this.metadataSourceType = value; }
        }

        /// <summary>
        /// Получение списка всех лямбд
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<SubscriptionLambda> GetLambdas()
        {
            return this.GetLambdasInternal().Where(sl => sl != null).Distinct();
        }

        private IEnumerable<SubscriptionLambda> GetLambdasInternal()
        {
            yield return this.Condition;
            yield return this.Generation;
            yield return this.DynamicSource;

            foreach (var si in this.SecurityItems)
            {
                yield return si.Source;
            }
        }

        public override string ToString()
        {
            return !string.IsNullOrEmpty(this.Code) ? this.Code : this.MetadataSourceType?.ToString();
        }

        #region IMailAddressContainer Members

        MailAddress IMailAddressContainer.Sender
        {
            get
            {
                var request = from email in this.SenderEmail.ToMaybe()

                              from name in this.SenderName.ToMaybe()

                              select new MailAddress(email, name);

                return request.GetValueOrDefault();
            }
        }

        #endregion

        ICollection<SubBusinessRole> IMaster<SubBusinessRole>.Details
        {
            get { return (ICollection<SubBusinessRole>)this.SubBusinessRoles; }
        }

        ICollection<SubscriptionSecurityItem> IMaster<SubscriptionSecurityItem>.Details
        {
            get { return (ICollection<SubscriptionSecurityItem>)this.SecurityItems; }
        }

        Subscription ISubscriptionElement.Subscription
        {
            get { return this; }
        }
    }
}
