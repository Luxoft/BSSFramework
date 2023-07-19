using Framework.Authorization.Domain;
using Framework.Persistent.Mapping;

namespace SampleSystem.Domain.TypedAuth;

[Table(Name = nameof(Permission), Schema = "auth")]
public class TypedAuthPermission : PersistentDomainObjectBase
{
    private readonly ICollection<TypedAuthPermissionBusinessUnit> businessUnitItems = new List<TypedAuthPermissionBusinessUnit>();

    private readonly ICollection<TypedAuthPermissionManagementUnit> managementUnitItems = new List<TypedAuthPermissionManagementUnit>();

    private readonly ICollection<TypedAuthPermissionLocation> locationItems = new List<TypedAuthPermissionLocation>();

    private readonly ICollection<TypedAuthPermissionEmployee> employeeItems = new List<TypedAuthPermissionEmployee>();


    public virtual IEnumerable<TypedAuthPermissionBusinessUnit> BusinessUnitItems => this.businessUnitItems;

    public virtual IEnumerable<TypedAuthPermissionManagementUnit> ManagementUnitItems => this.managementUnitItems;

    public virtual IEnumerable<TypedAuthPermissionLocation> LocationItems => this.locationItems;

    public virtual IEnumerable<TypedAuthPermissionEmployee> EmployeeItems => this.employeeItems;
}
