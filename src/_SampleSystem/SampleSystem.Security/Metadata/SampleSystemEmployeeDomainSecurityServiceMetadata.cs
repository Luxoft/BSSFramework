using Framework.SecuritySystem;
using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

using SampleSystem.Domain;
using SampleSystem.Security.Services;

namespace SampleSystem.Security.Metadata;

public class SampleSystemEmployeeDomainSecurityServiceMetadata
    : IDomainSecurityServiceMetadata<Employee>
{
    public static void Setup(IDomainSecurityServiceBuilder<Employee> builder) =>
        builder.SetView(SampleSystemSecurityOperation.EmployeeView)
               .SetView<CurrentEmployeeSecurityProvider<Employee>>()
               .SetEdit(SampleSystemSecurityOperation.EmployeeEdit)
               .SetPath(SecurityPath<Employee>.Create(employee => employee).And(employee => employee.CoreBusinessUnit).And(employee => employee.Location));
}
