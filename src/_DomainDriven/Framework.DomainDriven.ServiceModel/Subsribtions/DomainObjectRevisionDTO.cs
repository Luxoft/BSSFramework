using System.Runtime.Serialization;
using Framework.DomainDriven.DAL.Revisions;

namespace Framework.DomainDriven.ServiceModel.Subscriptions;

[DataContract(Name = "DomainObjectRevisionDTO{0}")]
public class DomainObjectRevisionDTO<TIdent>
{
    [DataMember]
    public TIdent Identity;
    [DataMember]
    public IEnumerable<DomainObjectRevisionInfoDTO<TIdent>> RevisionInfos;

    public DomainObjectRevisionDTO()
    {

    }
    public DomainObjectRevisionDTO(DomainObjectRevision<TIdent> source)
    {
        this.Identity = source.Identity;
        this.RevisionInfos = source.RevisionInfos.Select(z => new DomainObjectRevisionInfoDTO<TIdent>(z)).ToList();
    }
}
