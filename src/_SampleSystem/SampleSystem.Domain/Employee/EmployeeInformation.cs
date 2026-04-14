using Framework.BLL.Domain.ServiceRole;
using Framework.Core;
using Framework.Restriction;

namespace SampleSystem.Domain.Employee;

[BLLViewRole]
public class EmployeeInformation : Information
{
    private string personalEmail;

    [MaxLength(50)]
    public virtual string PersonalEmail
    {
        get => this.personalEmail.TrimNull();
        set => this.personalEmail = value.TrimNull();
    }
}
