using Framework.Authorization.SecuritySystem;
using Framework.SecuritySystem;

using SampleSystem.Domain;

using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;

namespace SampleSystem.ServiceEnvironment;

public static class SampleSystemEmployeeDomainSecurityServiceExtensions
{
    public static IDomainSecurityServiceRootBuilder<Guid> AddEmployee(this IDomainSecurityServiceRootBuilder<Guid> builder)
    {
        return builder.Add<Employee>(
            b =>
                b.SetView(SampleSystemSecurityOperation.EmployeeView)
                 .SetEdit(SampleSystemSecurityOperation.EmployeeEdit)
                 .SetPath(SecurityPath<Employee>.Create(employee => employee).And(employee => employee.CoreBusinessUnit).And(employee => employee.Location))
                 .Override<EmployeeSecurity>());
    }

    private class EmployeeSecurity : IOverrideSecurityFunctor<Employee>
    {
        private readonly IRunAsManager runAsManager;

        public EmployeeSecurity(IRunAsManager runAsManager) => this.runAsManager = runAsManager;

        public ISecurityProvider<Employee> Override(ISecurityProvider<Employee> baseProvider, SecurityOperation securityOperation)
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
    }
}
