using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DTO;
using Framework.Configuration.BLL;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.Events;

namespace Framework.Authorization.Events;

public class AuthorizationLocalDBEventMessageSender : LocalDBEventMessageSender<PersistentDomainObjectBase, EventDTOBase>
{
    private readonly IAuthorizationDTOMappingService mappingService;

    private readonly bool shrinkDto;

    public AuthorizationLocalDBEventMessageSender(IAuthorizationDTOMappingService mappingService, IConfigurationBLLContext configurationContext, string queueTag = "authDALQuery", bool shrinkDto = true)
            : base(configurationContext, queueTag)
    {
        this.mappingService = mappingService;
        this.shrinkDto = shrinkDto;
    }

    protected override EventDTOBase ToEventDTOBase<TDomainObject, TOperation>(IDomainOperationSerializeData<TDomainObject, TOperation> domainObjectEventArgs)
    {
        var dto = AuthorizationDomainEventDTOMapper<TDomainObject, TOperation>.MapToEventDTO(
         this.mappingService,
         domainObjectEventArgs.DomainObject,
         domainObjectEventArgs.Operation);

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
