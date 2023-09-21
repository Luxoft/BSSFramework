using Framework.Authorization.SecuritySystem;
using Framework.SecuritySystem;
using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

using SampleSystem.Domain;

namespace SampleSystem.ServiceEnvironment;

public class SampleSystemEmployeeDomainSecurityServiceMetadata : IDomainSecurityServiceMetadata<Employee>
{
    private readonly IRunAsManager runAsManager;

    public SampleSystemEmployeeDomainSecurityServiceMetadata(IRunAsManager runAsManager) => this.runAsManager = runAsManager;

    public ISecurityProvider<Employee> OverrideSecurityProvider(ISecurityProvider<Employee> baseProvider, SecurityOperation securityOperation)
    {
        if (securityOperation == SampleSystemSecurityOperation.EmployeeView)
        {
            return baseProvider.Or(employee => employee.Login == this.runAsManager.ActualPrincipal.Name);
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
