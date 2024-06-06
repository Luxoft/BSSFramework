using System.Runtime.Serialization;
using Framework.DomainDriven.DAL.Revisions;

namespace Framework.DomainDriven.ServiceModel.Subscriptions;

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
