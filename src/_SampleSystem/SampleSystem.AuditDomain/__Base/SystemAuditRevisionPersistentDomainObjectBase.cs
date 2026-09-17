using Framework.Application.Domain;

// ReSharper disable once CheckNamespace
namespace SampleSystem.AuditDomain;

public abstract class SystemAuditRevisionPersistentDomainObjectBase : IIdentityObject<long>
{
    private long id;

    public virtual long Id => this.id;
}
