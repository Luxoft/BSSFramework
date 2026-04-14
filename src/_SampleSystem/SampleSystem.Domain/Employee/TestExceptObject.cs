namespace SampleSystem.Domain.Employee;

public class TestExceptObject : AuditPersistentDomainObjectBase
{
    private Employee employee;

    public virtual Employee Employee
    {
        get => this.employee;
        set => this.employee = value;
    }
}
