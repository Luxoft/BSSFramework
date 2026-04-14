using Framework.Application.Domain;

namespace SampleSystem.AuditDomain.__Base;

public abstract class SystemAuditRevisionPersistentDomainObjectBase : IIdentityObject<long>
{
    private long id;

    protected SystemAuditRevisionPersistentDomainObjectBase()
    {
    }

    public virtual long Id => this.id;
}