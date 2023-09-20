using Framework.Authorization.SecuritySystem;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

using SampleSystem.Domain;

namespace SampleSystem.BLL;

public class SampleSystemEmployeeSecurityService : ContextDomainSecurityService<Employee, Guid>
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
