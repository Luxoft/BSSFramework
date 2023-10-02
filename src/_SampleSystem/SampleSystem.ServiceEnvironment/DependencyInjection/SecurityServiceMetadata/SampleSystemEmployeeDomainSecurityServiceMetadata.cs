using Framework.Authorization.SecuritySystem;
using Framework.SecuritySystem;
using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

using SampleSystem.Domain;

namespace SampleSystem.ServiceEnvironment;

public class SampleSystemEmployeeDomainSecurityServiceMetadata : IDomainSecurityServiceMetadata<Employee>
{
    private readonly IActualPrincipalSource actualPrincipalSource;

    public SampleSystemEmployeeDomainSecurityServiceMetadata(IActualPrincipalSource actualPrincipalSource) => this.actualPrincipalSource = actualPrincipalSource;

    public ISecurityProvider<Employee> OverrideSecurityProvider(ISecurityProvider<Employee> baseProvider, SecurityOperation securityOperation)
    {
        if (securityOperation == SampleSystemSecurityOperation.EmployeeView)
        {
            return baseProvider.Or(employee => employee.Login == this.actualPrincipalSource.ActualPrincipal.Name);
        }
        else
        {
            return baseProvider;
        }
    }

    public static void Setup(IDomainSecurityServiceBuilder<Employee> builder)
    {
        builder.SetView(SampleSystemSecurityOperation.EmployeeView)
               .SetEdit(SampleSystemSecurityOperation.EmployeeEdit)
               .SetPath(SecurityPath<Employee>.Create(employee => employee).And(employee => employee.CoreBusinessUnit).And(employee => employee.Location));
    }
}
