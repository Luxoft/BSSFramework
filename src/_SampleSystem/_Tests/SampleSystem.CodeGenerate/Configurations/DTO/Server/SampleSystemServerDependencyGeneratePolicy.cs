using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.Map;
using Framework.CodeGeneration.DTOGenerator.Server;
using Framework.CodeGeneration.DTOGenerator.Server.GeneratePolicy;
using Framework.CodeGeneration.GeneratePolicy;

using SampleSystem.Domain;

namespace SampleSystem.CodeGenerate.ServerDTO;

public class SampleSystemServerDependencyGeneratePolicy(IGeneratePolicy<RoleFileType> baseGeneratePolicy, IEnumerable<GenerateTypeMap> maps)
    : ServerDependencyGeneratePolicy(baseGeneratePolicy, maps)
{
    protected override bool InternalUsed(Type domainType, RoleFileType fileType)
    {
        if (domainType == typeof(Insurance) && fileType == ServerFileType.SimpleEventDTO)
        {
            return base.InternalUsed(domainType, fileType);
        }

        if (fileType == ServerFileType.BaseEventDTO)
        {
            return true;
        }

        if (base.InternalUsed(domainType, fileType))
        {
            return true;
        }
        else if (fileType == BaseFileType.SimpleDTO)
        {
            return base.InternalUsed(domainType, fileType) || this.Used(domainType, SampleSystemFileType.FullRefDTO)
                                                           || this.IsUsedProperty(SampleSystemFileType.FullRefDTO, domainType, fileType);
        }
        else if (fileType == BaseFileType.FullDTO)
        {
            return base.InternalUsed(domainType, fileType) || this.IsUsedProperty(SampleSystemFileType.FullRefDTO, domainType, fileType);
        }
        else
        {
            return false;
        }
    }
}
