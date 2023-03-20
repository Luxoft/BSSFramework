using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.Persistent;

namespace SampleSystem.Domain;

[BLLIntegrationSaveRole(AllowCreate = true, CustomImplementation = true)]
public class EmployeeCustomIntegrationSaveModel : DomainObjectIntegrationSaveModel<Employee>
{
    [DetailRole(DetailRole.Yes)]
    public override Employee SavingObject
    {
        get { return base.SavingObject; }
        set { base.SavingObject = value; }
    }
}
