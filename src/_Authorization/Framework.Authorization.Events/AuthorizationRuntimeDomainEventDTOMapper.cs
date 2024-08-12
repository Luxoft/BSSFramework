using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DTO;
using Framework.Events;
using Framework.Events.Legacy;

namespace Framework.Authorization.Events;

public class AuthorizationRuntimeDomainEventDTOMapper(
    IAuthorizationDTOMappingService mappingService,
    RuntimeDomainEventDTOConverter<PersistentDomainObjectBase, IAuthorizationDTOMappingService, EventDTOBase> converter,
    bool shrinkDto = true)
    : RuntimeDomainEventDTOMapper<PersistentDomainObjectBase, IAuthorizationDTOMappingService, EventDTOBase>(mappingService, converter)
{
    public override object Convert<TDomainObject>(TDomainObject domainObject, EventOperation domainObjectEvent)
    {
        var dto = base.Convert(domainObject, domainObjectEvent);

        if (shrinkDto)
        {
            switch (dto)
            {
                case PrincipalSaveEventDTO principalSave:
                    principalSave.Principal.Permissions = null;
                    break;

                case PrincipalRemoveEventDTO principalRemove:
                    dto = new PrincipalRemoveEventDTO { Principal = new PrincipalEventRichDTO { Id = principalRemove.Principal.Id } };
                    break;

                case BusinessRoleRemoveEventDTO businessRoleRemove:
                    dto = new BusinessRoleRemoveEventDTO { BusinessRole = new BusinessRoleEventRichDTO { Id = businessRoleRemove.BusinessRole.Id } };
                    break;

                case PermissionRemoveEventDTO permissionRemove:
                    dto = new PermissionRemoveEventDTO { Permission = new PermissionEventRichDTO { Id = permissionRemove.Permission.Id } };
                    break;
            }
        }

        return dto;
    }
}
