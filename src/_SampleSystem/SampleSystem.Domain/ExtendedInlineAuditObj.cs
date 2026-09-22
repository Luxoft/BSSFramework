namespace SampleSystem.Domain;

public class ExtendedInlineAuditObj : AuditPersistentDomainObjectBase
{
    private Employee.Employee? createdByEmployee;

    private Employee.Employee? modifiedByEmployee;

    /// <summary>
    ///     Сотрудник, создавший доменный объект
    /// </summary>
    public virtual Employee.Employee? CreatedByEmployee
    {
        get => this.createdByEmployee;
        protected internal set => this.createdByEmployee = value;
    }

    /// <summary>
    ///     Сотрудник, изменивший доменный объект
    /// </summary>
    public virtual Employee.Employee? ModifiedByEmployee
    {
        get => this.modifiedByEmployee;
        protected internal set => this.modifiedByEmployee = value;
    }
}
