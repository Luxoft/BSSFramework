using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.GeneratePolicy;
using Framework.CodeGeneration.DTOGenerator.Map;
using Framework.CodeGeneration.DTOGenerator.Server.FileType;
using Framework.CodeGeneration.GeneratePolicy;

namespace Framework.CodeGeneration.DTOGenerator.Server.GeneratePolicy;

public class ServerDependencyGeneratePolicy(IGeneratePolicy<RoleFileType> baseGeneratePolicy, IEnumerable<GenerateTypeMap> maps)
    : DependencyGeneratePolicy(baseGeneratePolicy, maps)
{
    protected override bool InternalUsed(Type domainType, RoleFileType fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        if (fileType == ServerFileType.BaseEventDTO)
        {
            return this.Maps.Any(map => map.FileType is DomainOperationEventDTOFileType && this.Used(map.DomainType, map.FileType));
        }
        else if (fileType == ServerFileType.RichEventDTO)
        {
            return this.Maps.Any(map => map.DomainType == domainType && map.FileType is DomainOperationEventDTOFileType && this.Used(map.DomainType, map.FileType))

                   || this.IsUsedProperty(ServerFileType.RichEventDTO, domainType, fileType, true);
        }
        else if (fileType == ServerFileType.SimpleEventDTO)
        {
            return this.IsUsedProperty(ServerFileType.RichEventDTO, domainType, fileType, false);
        }
        else if (base.InternalUsed(domainType, fileType))
        {
            return true;
        }
        else if (fileType == ServerFileType.RichIntegrationDTO)
        {
            return base.InternalUsed(domainType, fileType)

                   || this.IsUsedProperty(ServerFileType.RichIntegrationDTO, domainType, fileType, true);
        }
        else if (fileType == ServerFileType.SimpleIntegrationDTO)
        {
            return this.IsUsedProperty(ServerFileType.RichIntegrationDTO, domainType, fileType, false);
        }
        else
        {
            return false;
        }
    }
}
