using Framework.BLL.Domain.ServiceRole;
using Framework.Restriction;

namespace SampleSystem.Domain.Employee;

[BLLViewRole]
[BLLEventRole]
public class Information : BaseDirectory
{
    private string email = "";

    [MaxLength(50)]
    public virtual string Email
    {
        get => this.email;
        set => this.email = value;
    }
}
