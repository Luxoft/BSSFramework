using System;
using System.Runtime.Serialization;
using Framework.Core;
using Framework.DomainDriven.DAL.Revisions;

namespace Framework.DomainDriven.ServiceModel.Subscriptions;

[DataContract(Name = "ObjectModificationInfoDTO{0}")]
public class ObjectModificationInfoDTO<TIdent>
{
    [DataMember]
    public ModificationType ModificationType;

    [DataMember]
    public TIdent Identity;

    [DataMember]
    public TypeInfoDescriptionDTO TypeInfoDescription;

    [DataMember]
    public long Revision;


    public ObjectModificationInfoDTO(ObjectModificationInfo<TIdent> source)
    {
        this.ModificationType = source.ModificationType;
        this.Identity = source.Identity;
        this.TypeInfoDescription = new TypeInfoDescriptionDTO(source.TypeInfo);
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

    public override string ToString()
    {
        return $"Identity: {this.Identity}, ModificationType: {this.ModificationType}, Revision: {this.Revision}, TypeInfoDescription: {this.TypeInfoDescription}";
    }
}
