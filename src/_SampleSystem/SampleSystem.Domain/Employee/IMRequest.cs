using Framework.BLL.Domain.ServiceRole;
using Framework.Database.Mapping;
using Framework.Relations;
using Framework.Restriction;

namespace SampleSystem.Domain.Employee;

[BLLViewRole]
public class IMRequest : Information
{
    private string message = "";

    private IMRequestDetail oneToOneDetail = null!;

    [MaxLength(50)]
    public virtual string Message
    {
        get => this.message;
        set => this.message = value;
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
