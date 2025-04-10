using Framework.Persistent;

namespace SampleSystem.Domain;

public class TestRelativeEmployeeChildObject : AuditPersistentDomainObjectBase, IDetail<TestRelativeEmployeeParentObject>
{
    private readonly TestRelativeEmployeeParentObject master;

    private Employee employee;

    public TestRelativeEmployeeChildObject(TestRelativeEmployeeParentObject master)
    {
        this.master = master;
        this.master.AddDetail(this);
    }

    public TestRelativeEmployeeParentObject Master => this.master;

    public virtual Employee Employee
    {
        get => this.employee;
        set => this.employee = value;
    }
}
