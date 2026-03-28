using System.Runtime.Serialization;

using Framework.BLL.Domain.DAL.Revisions;

namespace Framework.BLL.DTOMapping.Domain;

[DataContract]
public class DefaultDomainObjectRevisionDTO : DomainObjectRevisionDTO<Guid>
{
    public DefaultDomainObjectRevisionDTO()
    {

    }

    public DefaultDomainObjectRevisionDTO(DomainObjectRevision<Guid> source)
            : base(source)
    {

    }
}
