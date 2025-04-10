using Framework.Persistent;

namespace SampleSystem.Domain.Ad;

public class AdGroupMember : AuditPersistentDomainObjectBase, IDetail<AdGroup>
{
    private readonly AdGroup group;

    private Employee employee;

    protected AdGroupMember()
    {
    }

    public AdGroupMember(AdGroup group)
    {
        this.group = group;
        this.group.AddDetail(this);
    }

    public virtual AdGroup Group => this.group;

    public virtual Employee Employee { get { return this.employee; } set { this.employee = value; } }

    AdGroup IDetail<AdGroup>.Master => this.Group;
}
