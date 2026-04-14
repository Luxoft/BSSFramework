using Framework.BLL.Domain.ServiceRole;
using Framework.Relations;

namespace SampleSystem.Domain;

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
