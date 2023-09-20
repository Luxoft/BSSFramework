using Framework.Authorization.SecuritySystem;
using Framework.SecuritySystem;

using SampleSystem.Domain;

using Framework.SecuritySystem.DependencyInjection.DomainSecurityServiceBuilder;
using Framework.SecuritySystem.Rules.Builders;

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
                 .SetCustomService<SampleSystemEmployeeSecurityService>());
    }

    private class SampleSystemEmployeeSecurityService : ContextDomainSecurityService<Employee, Guid>
    {
        private readonly IRunAsManager runAsManager;

        public SampleSystemEmployeeSecurityService(
            IDisabledSecurityProviderSource disabledSecurityProviderSource,
            ISecurityOperationResolver securityOperationResolver,
            IAuthorizationSystem<Guid> authorizationSystem,
            ISecurityExpressionBuilderFactory securityExpressionBuilderFactory,
            SecurityPath<Employee> securityPath,
            IRunAsManager runAsManager)

            : base(disabledSecurityProviderSource, securityOperationResolver, authorizationSystem, securityExpressionBuilderFactory, securityPath)
        {
            this.runAsManager = runAsManager;
        }

        protected override ISecurityProvider<Employee> CreateSecurityProvider(ContextSecurityOperation securityOperation)
        {
            var baseProvider = base.CreateSecurityProvider(securityOperation);

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
