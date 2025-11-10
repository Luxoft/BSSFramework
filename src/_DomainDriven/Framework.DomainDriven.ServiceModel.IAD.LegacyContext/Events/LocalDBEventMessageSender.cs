using CommonFramework;

using Framework.Configuration.BLL;
using Framework.Core;
using Framework.Events.Legacy;
using Framework.Persistent;

namespace Framework.DomainDriven.ServiceModel.IAD;

/// <summary>
/// Класс для отправки доменных евентов в локальную бд
/// </summary>
/// <typeparam name="TPersistentDomainObjectBase"></typeparam>
public class LocalDBEventMessageSender<TPersistentDomainObjectBase>(
    IDomainEventDTOMapper<TPersistentDomainObjectBase> eventDtoMapper,
    IConfigurationBLLContext configurationContext,
    LocalDBEventMessageSenderSettings<TPersistentDomainObjectBase>? settings = null)
    : EventDTOMessageSenderBase<TPersistentDomainObjectBase>
    where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
{
    private readonly LocalDBEventMessageSenderSettings<TPersistentDomainObjectBase> settings = settings ?? new LocalDBEventMessageSenderSettings<TPersistentDomainObjectBase>();

    public override async Task SendAsync<TDomainObject>(IDomainOperationSerializeData<TDomainObject> domainObjectEventArgs, CancellationToken cancellationToken)
    {
        var dto = domainObjectEventArgs.CustomSendObject
                  ?? eventDtoMapper.Convert(domainObjectEventArgs.DomainObject, domainObjectEventArgs.Operation);

        var serializedData = DataContractSerializerHelper.Serialize(dto);
        var dbEvent = new Configuration.Domain.DomainObjectEvent
        {
            SerializeData = serializedData,
            Size = serializedData.Length,
            SerializeType = dto.GetType().FullName!,
            DomainObjectId = domainObjectEventArgs.DomainObject.Id,
            Revision = configurationContext.GetCurrentRevision(),
            QueueTag = this.settings.QueueTag
        };

        configurationContext.GetDomainType(domainObjectEventArgs.DomainObjectType, false).Maybe(domainType =>
        {
            dbEvent.Operation = domainType.EventOperations.GetByName(domainObjectEventArgs.Operation.ToString());
        });

        configurationContext.Logics.Default.Create<Configuration.Domain.DomainObjectEvent>().Save(dbEvent);
    }
}
