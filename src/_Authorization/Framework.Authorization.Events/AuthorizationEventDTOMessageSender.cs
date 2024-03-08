using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DTO;
using Framework.Core;
using Framework.Events;

namespace Framework.Authorization.Events;

public class AuthorizationEventDTOMessageSender : EventDTOMessageSender<PersistentDomainObjectBase, EventDTOBase>
{
    private readonly IAuthorizationDTOMappingService mappingService;

    public AuthorizationEventDTOMessageSender(IAuthorizationDTOMappingService mappingService, IMessageSender<EventDTOBase> messageSender)
            : base(messageSender)
    {
        this.mappingService = mappingService;
    }

    protected override EventDTOBase ToEventDTOBase<TDomainObject>(IDomainOperationSerializeData<TDomainObject> domainObjectEventArgs)
    {
        return AuthorizationDomainEventDTOMapper<TDomainObject>.MapToEventDTO(
         this.mappingService,
         domainObjectEventArgs.DomainObject,
         domainObjectEventArgs.Operation);
    }
}
