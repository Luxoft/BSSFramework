using Framework.Database.Mapping;

namespace SampleSystem.Domain.MU;

[View]
public class ManagementUnitToAncestorChildView : AuditPersistentDomainObjectBase
{
    private ManagementUnit childOrAncestor;
    private ManagementUnit source;

    public virtual ManagementUnit ChildOrAncestor => this.childOrAncestor;

    public virtual ManagementUnit Source => this.source;
}
