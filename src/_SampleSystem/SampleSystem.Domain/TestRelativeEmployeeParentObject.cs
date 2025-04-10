using Framework.Persistent;

namespace SampleSystem.Domain;

public class TestRelativeEmployeeParentObject : AuditPersistentDomainObjectBase, IMaster<TestRelativeEmployeeChildObject>
{
    private readonly ICollection<TestRelativeEmployeeChildObject> children = new List<TestRelativeEmployeeChildObject>();

    public virtual IEnumerable<TestRelativeEmployeeChildObject> Children => this.children;

    ICollection<TestRelativeEmployeeChildObject> IMaster<TestRelativeEmployeeChildObject>.Details => (ICollection<TestRelativeEmployeeChildObject>)this.Children;
}
