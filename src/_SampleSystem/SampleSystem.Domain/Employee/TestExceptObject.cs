namespace SampleSystem.Domain.Employee;

public class TestExceptObject : AuditPersistentDomainObjectBase
{
    private Employee employee = null!;

    public virtual Employee Employee
    {
        get => this.employee;
        set => this.employee = value;
    }
}
