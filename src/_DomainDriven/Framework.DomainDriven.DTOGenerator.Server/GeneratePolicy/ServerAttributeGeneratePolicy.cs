using System;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class ServerAttributeGeneratePolicy : AttributeGeneratePolicy
{
    public override bool Used(Type domainType, RoleFileType fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        if (fileType is DomainOperationEventDTOFileType)
        {
            var operation = (fileType as DomainOperationEventDTOFileType).Operation;

            return domainType.GetEventOperations().Contains(operation);
        }
        else if (fileType == ServerFileType.RichEventDTO)
        {
            return domainType.HasAttribute<BLLIntegrationSaveRoleAttribute>();
        }
        else
        {
            return base.Used(domainType, fileType);
        }
    }
}
