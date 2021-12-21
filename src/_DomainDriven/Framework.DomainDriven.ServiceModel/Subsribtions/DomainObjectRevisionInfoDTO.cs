using System.Runtime.Serialization;
using Framework.DomainDriven.DAL.Revisions;

namespace Framework.DomainDriven.ServiceModel.Subscriptions
{
    [DataContract(Name = "DomainObjectRevisionInfoDTO{0}")]
    public class DomainObjectRevisionInfoDTO<TIdent> : PropertyRevisionDTOBase
    {
        public DomainObjectRevisionInfoDTO()
        {

        }
        public DomainObjectRevisionInfoDTO(DomainObjectRevisionInfo<TIdent> source) : base(source)
        {

        }
    }
}