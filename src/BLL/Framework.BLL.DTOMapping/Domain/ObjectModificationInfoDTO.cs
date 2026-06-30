using System.Runtime.Serialization;

using Framework.Database;
using Framework.Database.Domain;

namespace Framework.BLL.DTOMapping.Domain;

[DataContract(Name = "ObjectModificationInfoDTO{0}")]
public class ObjectModificationInfoDTO<TIdent>
{
    [DataMember]
    public ModificationType ModificationType { get; set; }

    [DataMember]
    public TIdent Identity { get; set; } = default!;

    [DataMember]
    public TypeInfoDescriptionDTO TypeInfoDescription { get; set; } = null!;

    [DataMember]
    public long Revision { get; set; }

    public ObjectModificationInfoDTO()
    {
    }

    public ObjectModificationInfoDTO(ObjectModificationInfo<TIdent> source)
    {
        this.ModificationType = source.ModificationType;
        this.Identity = source.Identity;
        this.TypeInfoDescription = new TypeInfoDescriptionDTO { Name = source.TypeInfo.Name, Namespace = source.TypeInfo.Namespace };
        this.Revision = source.Revision;
    }

    public override string ToString() => $"Identity: {this.Identity}, ModificationType: {this.ModificationType}, Revision: {this.Revision}, TypeInfoDescription: {this.TypeInfoDescription}";
}

