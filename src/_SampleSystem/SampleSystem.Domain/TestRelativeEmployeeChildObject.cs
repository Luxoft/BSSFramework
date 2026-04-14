using Framework.Relations;

namespace SampleSystem.Domain;

public class TestRelativeEmployeeChildObject : AuditPersistentDomainObjectBase, IDetail<TestRelativeEmployeeParentObject>
{
    private readonly TestRelativeEmployeeParentObject master;

    private Employee.Employee employee;

    protected TestRelativeEmployeeChildObject()
    {
    }

    public TestRelativeEmployeeChildObject(TestRelativeEmployeeParentObject master)
    {
        this.master = master;
        this.master.AddDetail(this);
    }

    public virtual TestRelativeEmployeeParentObject Master => this.master;

    public virtual Employee.Employee Employee
    {
        get => this.employee;
        set => this.employee = value;
    }
}
