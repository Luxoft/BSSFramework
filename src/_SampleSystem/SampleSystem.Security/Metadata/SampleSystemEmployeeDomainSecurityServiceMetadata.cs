using SecuritySystem;

using SampleSystem.Domain.Employee;

using SecuritySystem.DependencyInjection.Domain;

namespace SampleSystem.Security.Metadata;

public class SampleSystemEmployeeDomainSecurityServiceMetadata
    : IDomainSecurityServiceMetadata<Employee>
{
    public static void Setup(IDomainSecurityServiceSetup<Employee> setup) =>
        setup.SetView(SampleSystemSecurityOperation.EmployeeView.Or(DomainSecurityRule.CurrentUser))
             .SetEdit(SampleSystemSecurityOperation.EmployeeEdit)
             .SetPath(
                 SecurityPath<Employee>.Create(employee => employee)
                                       .And(employee => employee.CoreBusinessUnit)
                                       .And(employee => employee.Location));
}
