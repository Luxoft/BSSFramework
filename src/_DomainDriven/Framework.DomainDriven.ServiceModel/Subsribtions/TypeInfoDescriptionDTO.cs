using System.Runtime.Serialization;
using Framework.DomainDriven.DAL.Revisions;
using Framework.Persistent;

namespace Framework.DomainDriven.ServiceModel.Subscriptions
{
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
}
