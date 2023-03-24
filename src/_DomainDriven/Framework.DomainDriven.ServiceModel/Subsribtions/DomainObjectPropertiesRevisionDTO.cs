using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Framework.DomainDriven.DAL.Revisions;

namespace Framework.DomainDriven.ServiceModel.Subscriptions;

[DataContract(Name = "DomainObjectPropertiesRevisionDTO{0}")]
public class DomainObjectPropertiesRevisionDTO<TIdent, TPropertyReveision>
        where TPropertyReveision : PropertyRevisionDTOBase
{
    [DataMember] public string PropertyName;

    [DataMember] public TIdent Identity;

    [DataMember]
    public IEnumerable<TPropertyReveision> RevisionInfos = new List<TPropertyReveision>();

    public DomainObjectPropertiesRevisionDTO()
    {

    }
}
