namespace SampleSystem.Domain;

public class ExtendedInlineAuditObj : AuditPersistentDomainObjectBase
{
    private Employee.Employee? createdByEmployee;

    private Employee.Employee? modifiedByEmployee;

    /// <summary>
    ///     Employee who created the domain object
    /// </summary>
    public virtual Employee.Employee? CreatedByEmployee
    {
        get => this.createdByEmployee;
        protected internal set => this.createdByEmployee = value;
    }

    /// <summary>
    ///     Employee who modified the domain object
    /// </summary>
    public virtual Employee.Employee? ModifiedByEmployee
    {
        get => this.modifiedByEmployee;
        protected internal set => this.modifiedByEmployee = value;
    }
}
