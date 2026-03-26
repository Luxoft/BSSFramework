using System.Runtime.Serialization;

using Framework.BLL.Domain.DAL.Revisions;
using Framework.BLL.Domain.IdentityObject;

namespace Framework.BLL.DTOMapping.Domain;

[DataContract]
public class TypeInfoDescriptionDTO : IDomainType
{
    public TypeInfoDescriptionDTO()
    {
    }

    public TypeInfoDescriptionDTO(IDomainType domainType)
    {
        this.Name = domainType.Name;
        this.NameSpace = domainType.NameSpace;
    }

    [DataMember]
    public string NameSpace { get; set; }

    [DataMember]
    public string Name { get; set; }

    public void MapToDomainObject(TypeInfoDescription typeInfoDescription)
    {
        typeInfoDescription.Name = this.Name;
        typeInfoDescription.NameSpace = this.NameSpace;
    }

    public override string ToString()
    {
        return $"Name: {this.Name}, NameSpace: {this.NameSpace}";
    }
}
