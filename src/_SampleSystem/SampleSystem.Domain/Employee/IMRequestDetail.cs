namespace SampleSystem.Domain.Employee;

/// <summary>
/// Special detail for checking a OneToOne relation
/// </summary>
public class IMRequestDetail : AuditPersistentDomainObjectBase
{
    private IMRequest request = null!;

    protected IMRequestDetail()
    {
    }

    public IMRequestDetail(IMRequest request) => this.request = request;

    public virtual IMRequest Request
    {
        get => this.request;
        protected internal set => this.request = value;
    }
}
