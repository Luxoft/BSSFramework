using Framework.BLL.Domain.Attributes;

using SampleSystem.Domain.Models.Filters._Base;

namespace SampleSystem.Domain.Models.Filters;

[ViewDomainObject(typeof(Employee.Employee))]
public class EmployeeFilterModel : DomainObjectContextFilterModel<Employee.Employee>;
