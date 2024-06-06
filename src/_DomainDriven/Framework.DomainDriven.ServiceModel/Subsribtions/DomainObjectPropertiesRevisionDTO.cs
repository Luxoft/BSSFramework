using System.Runtime.Serialization;

namespace Framework.DomainDriven.ServiceModel.Subscriptions;

[DataContract(Name = "DomainObjectPropertiesRevisionDTO{0}")]
public class DomainObjectPropertiesRevisionDTO<TIdent, TPropertyReveision>
    where TPropertyReveision : PropertyRevisionDTOBase
{
    [DataMember]
    public string PropertyName { get; set; }

    [DataMember]
    public TIdent Identity { get; set; }

    [DataMember]
    public IEnumerable<TPropertyReveision> RevisionInfos { get; set; } = new List<TPropertyReveision>();
}
