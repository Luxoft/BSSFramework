using Framework.SecuritySystem;
using Framework.SecuritySystem.AccessDeniedExceptionService;
using Framework.SecuritySystem.Rules.Builders;

using SampleSystem.Domain;

namespace SampleSystem.BLL;

public partial class SampleSystemEmployeeSecurityService<TDomainObject, TBusinessUnit, TDepartment, TLocation, TEmployee>
{
    public SampleSystemEmployeeSecurityService(
            IAccessDeniedExceptionService<PersistentDomainObjectBase> accessDeniedExceptionService,
            IDisabledSecurityProviderContainer<PersistentDomainObjectBase> disabledSecurityProviderContainer,
            ISecurityOperationResolver<PersistentDomainObjectBase, SampleSystemSecurityOperationCode> securityOperationResolver,
            IAuthorizationSystem<Guid> authorizationSystem,
            ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> securityExpressionBuilderFactory,
            ISampleSystemSecurityPathContainer securityPathContainer,
            ISampleSystemBLLContext context)

            : base(accessDeniedExceptionService, disabledSecurityProviderContainer, securityOperationResolver, authorizationSystem, securityExpressionBuilderFactory)
    {
        this.securityPathContainer = securityPathContainer ?? throw new ArgumentNullException(nameof(securityPathContainer));
        this.Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public ISampleSystemBLLContext Context { get; }


    protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(ContextSecurityOperation<SampleSystemSecurityOperationCode> securityOperation)
    {
        var baseProvider = base.CreateSecurityProvider(securityOperation);

        if (securityOperation == SampleSystemSecurityOperation.EmployeeView)
        {
            return baseProvider.Or(employee => employee.Login == this.Context.Authorization.RunAsManager.PrincipalName, this.AccessDeniedExceptionService);
        }
        else
        {
            return baseProvider;
        }
    }
}
