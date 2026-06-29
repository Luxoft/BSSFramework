using System.Runtime.Serialization;

namespace Framework.BLL.DTOMapping.Domain;

[DataContract(Name = "DomainObjectPropertiesRevisionDTO{0}")]
public class DomainObjectPropertiesRevisionDTO<TIdent, TPropertyRevision>
    where TPropertyRevision : PropertyRevisionDTOBase
{
    [DataMember]
    public string PropertyName { get; set; } = null!;

    [DataMember]
    public TIdent Identity { get; set; } = default!;

    [DataMember]
    public List<TPropertyRevision> RevisionInfos { get; set; } = [];
}

