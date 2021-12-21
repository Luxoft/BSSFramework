using System;

using Framework.Configuration.BLL;
using Framework.Core;
using Framework.Events;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.DomainDriven.ServiceModel.IAD
{
    /// <summary>
    /// Класс для отправки доменных евентов в локальную бд
    /// </summary>
    /// <typeparam name="TBLLContext"></typeparam>
    /// <typeparam name="TPersistentDomainObjectBase"></typeparam>
    /// <typeparam name="TEventDTOBase"></typeparam>
    public abstract class LocalDBEventMessageSender<TBLLContext, TPersistentDomainObjectBase, TEventDTOBase> : EventDTOMessageSenderBase<TBLLContext, TPersistentDomainObjectBase, TEventDTOBase>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
        where TBLLContext : class
    {
        private readonly IConfigurationBLLContext configurationContext;

        private readonly string queueTag;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="context">Контекст системы</param>
        /// <param name="configurationContext">Контекст утилит</param>
        /// <param name="queueTag">Таг, маркирующий очередь евентов</param>
        protected LocalDBEventMessageSender([NotNull] TBLLContext context, [NotNull] Framework.Configuration.BLL.IConfigurationBLLContext configurationContext, [NotNull] string queueTag = "default")
            : base(context)
        {
            if (string.IsNullOrWhiteSpace(queueTag)) { throw new ArgumentException("Value cannot be null or whitespace.", nameof(queueTag)); }

            this.configurationContext = configurationContext ?? throw new ArgumentNullException(nameof(configurationContext));
            this.queueTag = queueTag;
        }

        public override void Send<TDomainObject, TOperation>(IDomainOperationSerializeData<TDomainObject, TOperation> domainObjectEventArgs, TransactionMessageMode sendMessageMode)
        {
            var dto = domainObjectEventArgs.CustomSendObject ?? this.ToEventDTOBase(domainObjectEventArgs);

            var serializedData = DataContractSerializerHelper.Serialize(dto);
            var dbEvent = new Framework.Configuration.Domain.DomainObjectEvent
            {
                SerializeData = serializedData,
                Size = serializedData.Length,
                SerializeType = dto.GetType().FullName,
                DomainObjectId = domainObjectEventArgs.DomainObject.Id,
                Revision = this.configurationContext.GetCurrentRevision(),
                QueueTag = this.queueTag
            };

            this.configurationContext.GetDomainType(domainObjectEventArgs.DomainObjectType, false).Maybe(domainType =>
            {
                dbEvent.Operation = domainType.EventOperations.GetByName(domainObjectEventArgs.Operation.ToString());
            });

            this.configurationContext.Logics.Default.Create<Framework.Configuration.Domain.DomainObjectEvent>().Save(dbEvent);
        }
    }
}
