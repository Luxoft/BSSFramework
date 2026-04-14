using Framework.BLL.Domain.ServiceRole;
using Framework.Relations;

using SampleSystem.Domain.Models.Integration._Base;

namespace SampleSystem.Domain.Models.Integration;

[BLLIntegrationSaveRole(AllowCreate = true, CustomImplementation = true)]
public class EmployeeCustomIntegrationSaveModel : DomainObjectIntegrationSaveModel<Employee.Employee>
{
    [DetailRole(DetailRole.Yes)]
    public override Employee.Employee SavingObject
    {
        get => base.SavingObject;
        set => base.SavingObject = value;
    }
}
