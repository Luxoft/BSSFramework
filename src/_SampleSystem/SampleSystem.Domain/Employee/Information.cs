using Framework.BLL.Domain.ServiceRole;
using Framework.Core;
using Framework.Restriction;

namespace SampleSystem.Domain;

[BLLViewRole]
[BLLEventRole]
public class Information : BaseDirectory
{
    private string email;

    [MaxLength(50)]
    public virtual string Email
    {
        get => this.email.TrimNull();
        set => this.email = value.TrimNull();
    }
}
