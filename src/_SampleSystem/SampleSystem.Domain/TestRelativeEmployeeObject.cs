namespace SampleSystem.Domain;

public class TestRelativeEmployeeObject : AuditPersistentDomainObjectBase
{
    private Employee employeeRef1;

    private Employee employeeRef2;

    public virtual Employee EmployeeRef1
    {
        get => this.employeeRef1;
        set => this.employeeRef1 = value;
    }


    public virtual Employee EmployeeRef2
    {
        get => this.employeeRef2;
        set => this.employeeRef2 = value;
    }
}
