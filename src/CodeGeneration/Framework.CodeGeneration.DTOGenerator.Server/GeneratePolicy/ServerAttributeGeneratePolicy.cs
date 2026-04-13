using Framework.BLL.Domain.ServiceRole;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileType;
using Framework.Core;

namespace Framework.CodeGeneration.DTOGenerator.Server.GeneratePolicy;

public class ServerAttributeGeneratePolicy(IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment> configuration)
    : AttributeGeneratePolicy(configuration.Environment.MetadataProxyProvider)
{
    public override bool Used(Type domainType, RoleFileType fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        if (fileType is DomainOperationEventDTOFileType domainOperationEventDTOFileType)
        {
            var operation = domainOperationEventDTOFileType.EventOperation;

            return configuration.DomainObjectEventMetadata.GetEventOperations(domainType).Contains(operation);
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
