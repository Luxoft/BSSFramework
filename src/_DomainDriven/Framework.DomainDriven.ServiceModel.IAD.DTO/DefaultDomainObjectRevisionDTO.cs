using System;
using System.Runtime.Serialization;
using Framework.DomainDriven.DAL.Revisions;
using Framework.DomainDriven.ServiceModel.Subscriptions;

namespace Framework.DomainDriven.ServiceModel.IAD;

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
