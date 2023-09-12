using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Restriction;
using Framework.Security;

namespace SampleSystem.Domain;

[BLLViewRole]
[ViewDomainObject(typeof(SampleSystemSecurityOperation), nameof(SampleSystemSecurityOperation.Disabled))]
public class IMRequest : Information
{
    private string message;

    private IMRequestDetail oneToOneDetail;

    [MaxLength(50)]
    public virtual string Message
    {
        get { return this.message.TrimNull(); }
        set { this.message = value.TrimNull(); }
    }

    [DetailRole(true)]
    [Mapping(IsOneToOne = true, CascadeMode = CascadeMode.Enabled)]
    [Required]
    public virtual IMRequestDetail OneToOneDetail
    {
        get => this.oneToOneDetail;
        set => this.oneToOneDetail = value;
    }
}
