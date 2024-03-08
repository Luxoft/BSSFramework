using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DTO;
using Framework.Events;
using Framework.Events.DTOMapper;

namespace Framework.Authorization.Events;

public class AuthorizationRuntimeDomainEventDTOMapper : RuntimeDomainEventDTOMapper<PersistentDomainObjectBase, IAuthorizationDTOMappingService, EventDTOBase>
{
    private readonly bool shrinkDto;

    public AuthorizationRuntimeDomainEventDTOMapper(
        IAuthorizationDTOMappingService mappingService,
        RuntimeDomainEventDTOConverter<PersistentDomainObjectBase, IAuthorizationDTOMappingService, EventDTOBase> converter,
        bool shrinkDto = true)
        : base(mappingService, converter)
    {
        this.shrinkDto = shrinkDto;
    }

    public override object Convert<TDomainObject>(TDomainObject domainObject, EventOperation eventOperation)
    {
        var dto = base.Convert(domainObject, eventOperation);

        if (this.shrinkDto)
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
