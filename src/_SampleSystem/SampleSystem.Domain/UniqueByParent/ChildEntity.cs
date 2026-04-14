using Framework.Restriction;

namespace SampleSystem.Domain.UniqueByParent;

[UniqueGroup(nameof(Parent))]
public class ChildEntity : AuditPersistentDomainObjectBase
{
    private readonly ParentEntity parent;

    [UniqueElement(nameof(Parent))]
    public virtual ParentEntity Parent => this.parent;
}
