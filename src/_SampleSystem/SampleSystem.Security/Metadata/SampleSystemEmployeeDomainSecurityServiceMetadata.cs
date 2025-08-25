using SecuritySystem;
using SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

using SampleSystem.Domain;

namespace SampleSystem.Security.Metadata;

public class SampleSystemEmployeeDomainSecurityServiceMetadata
    : IDomainSecurityServiceMetadata<Employee>
{
    public static void Setup(IDomainSecurityServiceBuilder<Employee> builder) =>
        builder.SetView(SampleSystemSecurityOperation.EmployeeView.Or(DomainSecurityRule.CurrentUser))
               .SetEdit(SampleSystemSecurityOperation.EmployeeEdit)
               .SetPath(
                   SecurityPath<Employee>.Create(employee => employee)
                                         .And(employee => employee.CoreBusinessUnit)
                                         .And(employee => employee.Location));
}
