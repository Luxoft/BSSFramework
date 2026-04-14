using Framework.BLL.Domain.Serialization;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.GeneratePolicy;

namespace SampleSystem.CodeGenerate.Configurations.DTO.Server;

public class SampleSystemEventDTORoleGeneratePolicy : IGeneratePolicy<RoleFileType>
{
    public bool Used(Type domainType, RoleFileType identity)
    {
        if (identity.Role == DTORole.Event)
        {
            return true;
        }

        return false;
    }
}
