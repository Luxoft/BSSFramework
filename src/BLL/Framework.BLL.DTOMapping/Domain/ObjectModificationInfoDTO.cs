using System.Runtime.Serialization;

using Framework.Core;
using Framework.Database.Domain;

namespace Framework.BLL.DTOMapping.Domain;

[DataContract(Name = "ObjectModificationInfoDTO{0}")]
public class ObjectModificationInfoDTO<TIdent>
{
    [DataMember]
    public ModificationType ModificationType { get; set; }

    [DataMember]
    public TIdent Identity { get; set; }

    [DataMember]
    public TypeInfoDescriptionDTO TypeInfoDescription { get; set; }

    [DataMember]
    public long Revision { get; set; }


    public ObjectModificationInfoDTO(ObjectModificationInfo<TIdent> source)
    {
        this.ModificationType = source.ModificationType;
        this.Identity = source.Identity;
        this.TypeInfoDescription = new TypeInfoDescriptionDTO { Name = source.TypeInfo.Name, NameSpace = source.TypeInfo.NameSpace };
        this.Revision = source.Revision;
    }

    public ObjectModificationInfoDTO()
    {

    }


    public void MapToDomainObject(ObjectModificationInfo<TIdent> source)
    {
        source.Identity = this.Identity;
        source.ModificationType = this.ModificationType;
        source.TypeInfo = new TypeInfoDescription().Self(z => this.TypeInfoDescription.MapToDomainObject(z));
        source.Revision = this.Revision;
    }

    public override string ToString() => $"Identity: {this.Identity}, ModificationType: {this.ModificationType}, Revision: {this.Revision}, TypeInfoDescription: {this.TypeInfoDescription}";
}
