using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.ServiceRole;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.GeneratePolicy;
using Framework.Core;
using Framework.Projection;
using Framework.ExtendedMetadata;

namespace Framework.CodeGeneration.DTOGenerator.Server.GeneratePolicy;

public class AttributeGeneratePolicy(IMetadataProxyProvider metadata) : IGeneratePolicy<RoleFileType>
{
    public virtual bool Used(Type domainType, RoleFileType fileType)
    {
        var domainTypeProxy = metadata.Wrap(domainType);

        if (fileType == BaseFileType.StrictDTO)
        {
            return domainTypeProxy.HasAttribute<BLLSaveRoleAttribute>(attr => attr.SaveType.HasFlag(BLLSaveType.Save));
        }
        else if (fileType == BaseFileType.UpdateDTO)
        {
            return domainTypeProxy.HasAttribute<BLLSaveRoleAttribute>(attr => attr.SaveType.HasFlag(BLLSaveType.Update));
        }
        else if (fileType == BaseFileType.RichDTO)
        {
            return domainTypeProxy.HasAttribute<BLLViewRoleAttribute>(attr => attr.All.Contains(MainDTOType.RichDTO));
        }
        else if (fileType == BaseFileType.FullDTO)
        {
            return domainTypeProxy.HasAttribute<BLLViewRoleAttribute>(attr => attr.All.Contains(MainDTOType.FullDTO));
        }
        else if (fileType == BaseFileType.SimpleDTO)
        {
            return domainTypeProxy.HasAttribute<BLLViewRoleAttribute>(attr => attr.All.Contains(MainDTOType.SimpleDTO));
        }
        else if (fileType == BaseFileType.VisualDTO)
        {
            return domainTypeProxy.HasAttribute<BLLViewRoleAttribute>(attr => attr.All.Contains(MainDTOType.VisualDTO));
        }
        else if (fileType == BaseFileType.IdentityDTO)
        {
            return domainTypeProxy.HasAttribute<BLLSaveRoleAttribute>()
                   || domainTypeProxy.HasAttribute<BLLRemoveRoleAttribute>()
                   || domainTypeProxy.HasAttribute<BLLViewRoleAttribute>();
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
