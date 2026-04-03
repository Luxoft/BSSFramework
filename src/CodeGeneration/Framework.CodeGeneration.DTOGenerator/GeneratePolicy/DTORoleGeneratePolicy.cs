using Framework.BLL.Domain.Serialization;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.GeneratePolicy;

namespace Framework.CodeGeneration.DTOGenerator.GeneratePolicy;

public class DTORoleGeneratePolicy(DTORole filter, ClientDTORole clientFilter = ClientDTORole.Main | ClientDTORole.Strict | ClientDTORole.Projection)
    : IGeneratePolicy<RoleFileType>
{
    public bool Used(Type domainType, RoleFileType fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        if (fileType is MainDTOFileType)
        {
            if (!clientFilter.HasFlag(ClientDTORole.Main))
            {
                return false;
            }
        }
        else if (fileType == BaseFileType.StrictDTO)
        {
            if (!clientFilter.HasFlag(ClientDTORole.Strict))
            {
                return false;
            }
        }
        else if (fileType == BaseFileType.UpdateDTO)
        {
            if (!clientFilter.HasFlag(ClientDTORole.Update))
            {
                return false;
            }
        }
        else if (fileType == BaseFileType.ProjectionDTO)
        {
            if (!clientFilter.HasFlag(ClientDTORole.Projection))
            {
                return false;
            }
        }

        return filter.HasFlag(fileType.Role);
    }
}
