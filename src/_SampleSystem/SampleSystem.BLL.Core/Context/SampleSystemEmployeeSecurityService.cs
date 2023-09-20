using Framework.Authorization.SecuritySystem;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

using SampleSystem.Domain;

namespace SampleSystem.BLL;

public class SampleSystemEmployeeSecurityService : ContextDomainSecurityService<PersistentDomainObjectBase, Employee, Guid>
{
    private readonly IRunAsManager runAsManager;

    public SampleSystemEmployeeSecurityService(
            IDisabledSecurityProviderSource disabledSecurityProviderSource,
            ISecurityOperationResolver<PersistentDomainObjectBase> securityOperationResolver,
            IAuthorizationSystem<Guid> authorizationSystem,
            ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> securityExpressionBuilderFactory,
            IRunAsManager runAsManager)

            : base(disabledSecurityProviderSource, securityOperationResolver, authorizationSystem, securityExpressionBuilderFactory)
    {
        this.runAsManager = runAsManager;
    }

    protected override SecurityPath<Employee> GetSecurityPath() => SecurityPath<Employee>.Create(employee => employee)
        .And(employee => employee.CoreBusinessUnit).And(employee => employee.Location);

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
