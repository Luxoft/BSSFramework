using System.Runtime.Serialization;

using Framework.BLL.Domain.DAL.Revisions;

namespace Framework.BLL.DTOMapping.Domain;

[DataContract(Name = "DomainObjectRevisionInfoDTO{0}")]
public class DomainObjectRevisionInfoDTO<TIdent> : PropertyRevisionDTOBase
{
    public DomainObjectRevisionInfoDTO()
    {

    }
    public DomainObjectRevisionInfoDTO(DomainObjectRevisionInfo<TIdent> source) : base(source)
    {

    }
}
