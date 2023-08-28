using Framework.Authorization.BLL;
using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DTO;
using Framework.Core;
using Framework.Events;

namespace Framework.Authorization.Events;

public class AuthorizationEventDTOMessageSender : EventDTOMessageSender<IAuthorizationBLLContext, PersistentDomainObjectBase, EventDTOBase>
{
    public AuthorizationEventDTOMessageSender(IAuthorizationBLLContext context, IMessageSender<EventDTOBase> messageSender)
            : base(context, messageSender)
    {
    }

    protected override EventDTOBase ToEventDTOBase<TDomainObject, TOperation>(IDomainOperationSerializeData<TDomainObject, TOperation> domainObjectEventArgs)
    {
        return AuthorizationDomainEventDTOMapper<TDomainObject, TOperation>.MapToEventDTO(
         new AuthorizationServerPrimitiveDTOMappingService(this.Context),
         domainObjectEventArgs.DomainObject,
         domainObjectEventArgs.Operation);
    }
}
