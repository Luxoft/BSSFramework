using Framework.Persistent;

namespace SampleSystem.Domain.Ad;

public class AdGroup : AuditPersistentDomainObjectBase, IMaster<AdGroupMember>
{
    private readonly ICollection<AdGroupMember> members = new List<AdGroupMember>();

    public virtual IEnumerable<AdGroupMember> Members => this.members;

    ICollection<AdGroupMember> IMaster<AdGroupMember>.Details => (ICollection<AdGroupMember>)this.Members;
}
