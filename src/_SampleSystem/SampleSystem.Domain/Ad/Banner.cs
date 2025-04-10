using Framework.Persistent;

namespace SampleSystem.Domain.Ad;

public class Banner : AuditPersistentDomainObjectBase, IMaster<BannerAccess>
{
    private readonly ICollection<BannerAccess> accesses = new List<BannerAccess>();

    public virtual IEnumerable<BannerAccess> Accesses => this.accesses;

    ICollection<BannerAccess> IMaster<BannerAccess>.Details => (ICollection<BannerAccess>)this.Accesses;
}
