using Framework.Application.Domain;
using Framework.Database.Domain;

// ReSharper disable once CheckNamespace
namespace SampleSystem.AuditDomain;

public abstract class SystemAuditPersistentDomainObjectBase : IIdentityObject<Guid>
{
    private AuditIdentifier identifier;

    private readonly SampleSystemAuditRevisionEntity revision = null!;

    private Guid id;

    private AuditRevisionType revType;

    private DateTime? modifyDate;

    private string modifiedBy = null!;

    protected SystemAuditPersistentDomainObjectBase()
    {
    }

    public virtual AuditIdentifier Identifier
    {
        get => this.identifier;
        protected internal set => this.identifier = value;
    }


    public virtual SampleSystemAuditRevisionEntity Revision => this.revision;

    public virtual Guid Id => this.id;


    public virtual AuditRevisionType RevType => this.revType;

    public virtual DateTime? ModifyDate => this.modifyDate;

    public virtual string ModifiedBy => this.modifiedBy;
}
