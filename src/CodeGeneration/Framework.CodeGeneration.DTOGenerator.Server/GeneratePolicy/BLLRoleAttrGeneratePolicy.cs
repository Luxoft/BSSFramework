using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.ServiceRole;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.GeneratePolicy;
using Framework.Core;
using Framework.Projection;
using Framework.Projection.ExtendedMetadata;

namespace Framework.CodeGeneration.DTOGenerator.Server.GeneratePolicy;

public class AttributeGeneratePolicy(IDomainTypeRootExtendedMetadata metadata) : IGeneratePolicy<RoleFileType>
{
    public virtual bool Used(Type domainType, RoleFileType fileType)
    {
        var domainTypeAttributeProvider = metadata.GetType(domainType);

        if (fileType == BaseFileType.StrictDTO)
        {
            return domainTypeAttributeProvider.HasAttribute<BLLSaveRoleAttribute>(attr => attr.SaveType.HasFlag(BLLSaveType.Save));
        }
        else if (fileType == BaseFileType.UpdateDTO)
        {
            return domainTypeAttributeProvider.HasAttribute<BLLSaveRoleAttribute>(attr => attr.SaveType.HasFlag(BLLSaveType.Update));
        }
        else if (fileType == BaseFileType.RichDTO)
        {
            return domainTypeAttributeProvider.HasAttribute<BLLViewRoleAttribute>(attr => attr.All.Contains(MainDTOType.RichDTO));
        }
        else if (fileType == BaseFileType.FullDTO)
        {
            return domainTypeAttributeProvider.HasAttribute<BLLViewRoleAttribute>(attr => attr.All.Contains(MainDTOType.FullDTO));
        }
        else if (fileType == BaseFileType.SimpleDTO)
        {
            return domainTypeAttributeProvider.HasAttribute<BLLViewRoleAttribute>(attr => attr.All.Contains(MainDTOType.SimpleDTO));
        }
        else if (fileType == BaseFileType.VisualDTO)
        {
            return domainTypeAttributeProvider.HasAttribute<BLLViewRoleAttribute>(attr => attr.All.Contains(MainDTOType.VisualDTO));
        }
        else if (fileType == BaseFileType.IdentityDTO)
        {
            return domainTypeAttributeProvider.HasAttribute<BLLSaveRoleAttribute>()
                   || domainTypeAttributeProvider.HasAttribute<BLLRemoveRoleAttribute>()
                   || domainTypeAttributeProvider.HasAttribute<BLLViewRoleAttribute>();
        }
        else if (fileType == BaseFileType.ProjectionDTO)
        {
            return domainType.IsProjection();
        }
        else
        {
            return false;
        }
    }
}
