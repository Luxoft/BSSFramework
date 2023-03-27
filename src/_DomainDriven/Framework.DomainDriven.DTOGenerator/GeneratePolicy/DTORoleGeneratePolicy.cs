using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Serialization;

namespace Framework.DomainDriven.DTOGenerator;

public class DTORoleGeneratePolicy : IGeneratePolicy<RoleFileType>
{
    private readonly DTORole _filter;

    private readonly ClientDTORole _clientFilter;


    public DTORoleGeneratePolicy(DTORole filter, ClientDTORole clientFilter = ClientDTORole.Main | ClientDTORole.Strict | ClientDTORole.Projection)
    {
        this._filter = filter;
        this._clientFilter = clientFilter;
    }


    public bool Used(Type domainType, RoleFileType fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));
        if (fileType == null) throw new ArgumentNullException(nameof(fileType));

        if (fileType is IMainDTOFileType)
        {
            if (!this._clientFilter.HasFlag(ClientDTORole.Main))
            {
                return false;
            }
        }
        else if (fileType == FileType.StrictDTO)
        {
            if (!this._clientFilter.HasFlag(ClientDTORole.Strict))
            {
                return false;
            }
        }
        else if (fileType == FileType.UpdateDTO)
        {
            if (!this._clientFilter.HasFlag(ClientDTORole.Update))
            {
                return false;
            }
        }
        else if (fileType == FileType.ProjectionDTO)
        {
            if (!this._clientFilter.HasFlag(ClientDTORole.Projection))
            {
                return false;
            }
        }

        return this._filter.HasFlag(fileType.Role);
    }
}
