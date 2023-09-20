using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

namespace SampleSystem.BLL;

public partial class SampleSystemEmployeeSecurityService<TDomainObject, TBusinessUnit, TDepartment, TLocation, TEmployee>
{
    public SampleSystemEmployeeSecurityService(
            IDisabledSecurityProviderSource disabledSecurityProviderSource,
            ISecurityOperationResolver securityOperationResolver,
            IAuthorizationSystem<Guid> authorizationSystem,
            ISecurityExpressionBuilderFactory securityExpressionBuilderFactory,
            ISampleSystemSecurityPathContainer securityPathContainer,
            ISampleSystemBLLContext context)

            : base(disabledSecurityProviderSource, securityOperationResolver, authorizationSystem, securityExpressionBuilderFactory, securityPathContainer.GetEmployeeSecurityPath<TDomainObject, TBusinessUnit, TDepartment, TLocation, TEmployee>())
    {
        this.Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public ISampleSystemBLLContext Context { get; }


    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(ContextSecurityOperation securityOperation)
    {
        var baseProvider = base.CreateSecurityProvider(securityOperation);

        if (securityOperation == SampleSystemSecurityOperation.EmployeeView)
        {
            return baseProvider.Or(employee => employee.Login == this.Context.Authorization.RunAsManager.ActualPrincipal.Name);
        }
        else
        {
            return baseProvider;
        }
    }
}
