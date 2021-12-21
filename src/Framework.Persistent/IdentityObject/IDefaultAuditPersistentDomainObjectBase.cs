using System;

namespace Framework.Persistent
{
    public interface IDefaultAuditPersistentDomainObjectBase : IDefaultIdentityObject, IAuditPersistentDomainObjectBase<Guid>
    {

    }
}