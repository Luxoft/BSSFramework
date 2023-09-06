using Framework.SecuritySystem.Rules.Builders;

namespace Framework.SecuritySystem.DiTests
{
    public class EmployeeSecurityService : ContextDomainSecurityService<PersistentDomainObjectBase, Employee, Guid, ExampleSecurityOperation>
    {
        public EmployeeSecurityService(
            IDisabledSecurityProviderSource<PersistentDomainObjectBase> disabledSecurityProviderSource,
            ISecurityOperationResolver<PersistentDomainObjectBase, ExampleSecurityOperation> securityOperationResolver,
            IAuthorizationSystem<Guid> authorizationSystem,
            ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> securityExpressionBuilderFactory)

            : base(accessDeniedExceptionService, disabledSecurityProviderSource, securityOperationResolver, authorizationSystem, securityExpressionBuilderFactory)
        {
        }

        protected override SecurityPath<PersistentDomainObjectBase, Employee, Guid> GetSecurityPath()
        {
            return SecurityPath<PersistentDomainObjectBase, Employee, Guid>.Create(v => v.BusinessUnit);
        }
    }
}
