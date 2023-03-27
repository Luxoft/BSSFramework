using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

using JetBrains.Annotations;

using SampleSystem.Domain;

namespace SampleSystem.BLL;

public partial class SampleSystemEmployeeSecurityService<TDomainObject, TBusinessUnit, TDepartment, TLocation, TEmployee>
{
    public SampleSystemEmployeeSecurityService(
            [NotNull] IAccessDeniedExceptionService<PersistentDomainObjectBase> accessDeniedExceptionService,
            [NotNull] IDisabledSecurityProviderContainer<PersistentDomainObjectBase> disabledSecurityProviderContainer,
            [NotNull] ISecurityOperationResolver<PersistentDomainObjectBase, SampleSystemSecurityOperationCode> securityOperationResolver,
            [NotNull] IAuthorizationSystem<Guid> authorizationSystem,
            [NotNull] ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> securityExpressionBuilderFactory,
            [NotNull] ISampleSystemSecurityPathContainer securityPathContainer,
            [NotNull] ISampleSystemBLLContext context)

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
