using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.ServiceRole;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.GeneratePolicy;
using Framework.Core;
using Framework.Projection;

namespace Framework.CodeGeneration.DTOGenerator.Server.GeneratePolicy;

public class AttributeGeneratePolicy : IGeneratePolicy<RoleFileType>
{
    protected AttributeGeneratePolicy()
    {

    }

    public virtual bool Used(Type domainType, RoleFileType fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        if (fileType == BaseFileType.StrictDTO)
        {
            return domainType.HasAttribute<BLLSaveRoleAttribute>(attr => attr.SaveType.HasFlag(BLLSaveType.Save));
        }
        else if (fileType == BaseFileType.UpdateDTO)
        {
            return domainType.HasAttribute<BLLSaveRoleAttribute>(attr => attr.SaveType.HasFlag(BLLSaveType.Update));
        }
        else if (fileType == BaseFileType.RichDTO)
        {
            return domainType.HasAttribute<BLLViewRoleAttribute>(attr => attr.All.Contains(MainDTOType.RichDTO));
        }
        else if (fileType == BaseFileType.FullDTO)
        {
            return domainType.HasAttribute<BLLViewRoleAttribute>(attr => attr.All.Contains(MainDTOType.FullDTO));
        }
        else if (fileType == BaseFileType.SimpleDTO)
        {
            return domainType.HasAttribute<BLLViewRoleAttribute>(attr => attr.All.Contains(MainDTOType.SimpleDTO));
        }
        else if (fileType == BaseFileType.VisualDTO)
        {
            return domainType.HasAttribute<BLLViewRoleAttribute>(attr => attr.All.Contains(MainDTOType.VisualDTO));
        }
        else if (fileType == BaseFileType.IdentityDTO)
        {
            return domainType.HasAttribute<BLLSaveRoleAttribute>()
                   || domainType.HasAttribute<BLLRemoveRoleAttribute>()
                   || domainType.HasAttribute<BLLViewRoleAttribute>();
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

    public static readonly AttributeGeneratePolicy Default = new AttributeGeneratePolicy();
}
