using Framework.Persistent;

namespace SampleSystem.Domain.Ad;

public class BannerAccess : AuditPersistentDomainObjectBase, IDetail<Banner>
{
    private readonly Banner banner;

    private AdGroup group;

    private bool accessFlag;

    protected BannerAccess()
    {
    }

    public BannerAccess(Banner banner)
    {
        this.banner = banner;
        this.banner.AddDetail(this);
    }

    public virtual Banner Banner => this.banner;

    public virtual AdGroup Group { get { return this.group; } set { this.group = value; } }

    public virtual bool AccessFlag { get { return this.accessFlag; } set { this.accessFlag = value; } }

    Banner IDetail<Banner>.Master => this.Banner;
}
