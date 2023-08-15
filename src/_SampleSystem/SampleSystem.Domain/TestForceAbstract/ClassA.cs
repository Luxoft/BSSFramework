using Framework.DomainDriven.Attributes;
using Framework.Persistent.Mapping;

namespace SampleSystem.Domain.TestForceAbstract;

[NotAuditedClass]
public class ClassA : PersistentDomainObjectBase
{
    private int value;

    private ICollection<ClassAChild> child = new List<ClassAChild>();

    public virtual int Value
    {
        get { return this.value; }
        set { this.value = value; }
    }

    public virtual IEnumerable<ClassAChild> Child
    {
        get { return this.child; }
    }
}
