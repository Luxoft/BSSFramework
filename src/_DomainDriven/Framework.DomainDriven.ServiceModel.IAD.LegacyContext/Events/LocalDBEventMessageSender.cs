using Framework.Configuration.BLL;
using Framework.Core;
using Framework.Events;
using Framework.Persistent;

namespace Framework.DomainDriven.ServiceModel.IAD;

/// <summary>
/// Класс для отправки доменных евентов в локальную бд
/// </summary>
/// <typeparam name="TPersistentDomainObjectBase"></typeparam>
/// <typeparam name="TEventDTOBase"></typeparam>
public abstract class LocalDBEventMessageSender<TPersistentDomainObjectBase, TEventDTOBase> : EventDTOMessageSenderBase<TPersistentDomainObjectBase, TEventDTOBase>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
{
    private readonly IConfigurationBLLContext configurationContext;

    private readonly string queueTag;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="configurationContext">Контекст утилит</param>
    /// <param name="queueTag">Таг, маркирующий очередь евентов</param>
    protected LocalDBEventMessageSender(IConfigurationBLLContext configurationContext, string queueTag = "default")
    {
        if (string.IsNullOrWhiteSpace(queueTag)) { throw new ArgumentException("Value cannot be null or whitespace.", nameof(queueTag)); }

        this.configurationContext = configurationContext ?? throw new ArgumentNullException(nameof(configurationContext));
        this.queueTag = queueTag;
    }

    public override void Send<TDomainObject>(IDomainOperationSerializeData<TDomainObject> domainObjectEventArgs)
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
