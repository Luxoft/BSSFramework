using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Generation.Domain;
using Framework.Projection;
using Framework.Transfering;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class AttributeGeneratePolicy : IGeneratePolicy<RoleFileType>
{
    protected AttributeGeneratePolicy()
    {

    }

    public virtual bool Used(Type domainType, RoleFileType fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        if (fileType == FileType.StrictDTO)
        {
            return domainType.HasAttribute<BLLSaveRoleAttribute>(attr => attr.SaveType.HasFlag(BLLSaveType.Save));
        }
        else if (fileType == FileType.UpdateDTO)
        {
            return domainType.HasAttribute<BLLSaveRoleAttribute>(attr => attr.SaveType.HasFlag(BLLSaveType.Update));
        }
        else if (fileType == FileType.RichDTO)
        {
            return domainType.HasAttribute<BLLViewRoleAttribute>(attr => attr.All.Contains(MainDTOType.RichDTO));
        }
        else if (fileType == FileType.FullDTO)
        {
            return domainType.HasAttribute<BLLViewRoleAttribute>(attr => attr.All.Contains(MainDTOType.FullDTO));
        }
        else if (fileType == FileType.SimpleDTO)
        {
            return domainType.HasAttribute<BLLViewRoleAttribute>(attr => attr.All.Contains(MainDTOType.SimpleDTO));
        }
        else if (fileType == FileType.VisualDTO)
        {
            return domainType.HasAttribute<BLLViewRoleAttribute>(attr => attr.All.Contains(MainDTOType.VisualDTO));
        }
        else if (fileType == FileType.IdentityDTO)
        {
            return domainType.HasAttribute<BLLSaveRoleAttribute>()
                   || domainType.HasAttribute<BLLRemoveRoleAttribute>()
                   || domainType.HasAttribute<BLLViewRoleAttribute>();
        }
        else if (fileType == FileType.ProjectionDTO)
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
