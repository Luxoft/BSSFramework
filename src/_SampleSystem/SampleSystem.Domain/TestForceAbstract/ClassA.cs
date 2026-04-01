using Framework.Database.Mapping;

namespace SampleSystem.Domain.TestForceAbstract;

[NotAuditedClass]
public class ClassA : PersistentDomainObjectBase
{
    private int value;

    private ICollection<ClassAChild> child = new List<ClassAChild>();

    public virtual int Value
    {
        get => this.value;
        set => this.value = value;
    }

    public virtual IEnumerable<ClassAChild> Child => this.child;
}
