using System.Runtime.Serialization;

using Framework.BLL.Domain.DAL.Revisions;

namespace Framework.BLL.DTOMapping.Domain;

[DataContract(Name = "DomainObjectRevisionDTO{0}")]
public class DomainObjectRevisionDTO<TIdent>
{
    [DataMember]
    public TIdent Identity { get; set; }

    [DataMember]
    public IEnumerable<DomainObjectRevisionInfoDTO<TIdent>> RevisionInfos { get; set; }

    public DomainObjectRevisionDTO()
    {

    }
    public DomainObjectRevisionDTO(DomainObjectRevision<TIdent> source)
    {
        this.Identity = source.Identity;
        this.RevisionInfos = source.RevisionInfos.Select(z => new DomainObjectRevisionInfoDTO<TIdent>(z)).ToList();
    }
}
