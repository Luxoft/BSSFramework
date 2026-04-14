namespace SampleSystem.Domain;

public class TestRelativeEmployeeObject : AuditPersistentDomainObjectBase
{
    private Employee.Employee employeeRef1;

    private Employee.Employee employeeRef2;

    public virtual Employee.Employee EmployeeRef1
    {
        get => this.employeeRef1;
        set => this.employeeRef1 = value;
    }


    public virtual Employee.Employee EmployeeRef2
    {
        get => this.employeeRef2;
        set => this.employeeRef2 = value;
    }
}
