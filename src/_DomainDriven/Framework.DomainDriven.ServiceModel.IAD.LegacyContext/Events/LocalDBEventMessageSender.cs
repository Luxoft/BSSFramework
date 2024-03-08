using Framework.Configuration.BLL;
using Framework.Core;
using Framework.Events;
using Framework.Events.DTOMapper;
using Framework.Persistent;

namespace Framework.DomainDriven.ServiceModel.IAD;

/// <summary>
/// Класс для отправки доменных евентов в локальную бд
/// </summary>
/// <typeparam name="TPersistentDomainObjectBase"></typeparam>
public class LocalDBEventMessageSender<TPersistentDomainObjectBase> : EventDTOMessageSenderBase<TPersistentDomainObjectBase>
    where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
{
    private readonly IDomainEventDTOMapper<TPersistentDomainObjectBase> eventDtoMapper;

    private readonly IConfigurationBLLContext configurationContext;

    private readonly LocalDBEventMessageSenderSettings<TPersistentDomainObjectBase> settings;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="configurationContext">Контекст утилит</param>
    public LocalDBEventMessageSender(
        IDomainEventDTOMapper<TPersistentDomainObjectBase> eventDtoMapper,
        IConfigurationBLLContext configurationContext,
        LocalDBEventMessageSenderSettings<TPersistentDomainObjectBase> settings)
    {
        this.eventDtoMapper = eventDtoMapper;
        this.configurationContext = configurationContext;
        this.settings = settings;
    }

    public override void Send<TDomainObject>(IDomainOperationSerializeData<TDomainObject> domainObjectEventArgs)
    {
        var dto = domainObjectEventArgs.CustomSendObject
                  ?? this.eventDtoMapper.Convert(domainObjectEventArgs.DomainObject, domainObjectEventArgs.Operation);

        var serializedData = DataContractSerializerHelper.Serialize(dto);
        var dbEvent = new Framework.Configuration.Domain.DomainObjectEvent
                      {
                              SerializeData = serializedData,
                              Size = serializedData.Length,
                              SerializeType = dto.GetType().FullName,
                              DomainObjectId = domainObjectEventArgs.DomainObject.Id,
                              Revision = this.configurationContext.GetCurrentRevision(),
                              QueueTag = this.settings.QueueTag
                      };

        this.configurationContext.GetDomainType(domainObjectEventArgs.DomainObjectType, false).Maybe(domainType =>
        {
            dbEvent.Operation = domainType.EventOperations.GetByName(domainObjectEventArgs.Operation.ToString());
        });

        this.configurationContext.Logics.Default.Create<Framework.Configuration.Domain.DomainObjectEvent>().Save(dbEvent);
    }
}
