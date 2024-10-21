using Framework.Security;

namespace SampleSystem.Domain.Models.Filters;

[ViewDomainObject(typeof(Employee))]
public class EmployeeFilterModel : DomainObjectContextFilterModel<Employee>
{
}
