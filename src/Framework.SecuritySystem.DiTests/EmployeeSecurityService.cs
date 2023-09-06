using Framework.SecuritySystem.Rules.Builders;

namespace Framework.SecuritySystem.DiTests
{
    public class EmployeeSecurityService : ContextDomainSecurityService<PersistentDomainObjectBase, Employee, Guid, ExampleSecurityOperation>
    {
        public EmployeeSecurityService(
            IDisabledSecurityProviderSource disabledSecurityProviderSource,
            ISecurityOperationResolver<PersistentDomainObjectBase, ExampleSecurityOperation> securityOperationResolver,
            IAuthorizationSystem<Guid> authorizationSystem,
            ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> securityExpressionBuilderFactory)

            : base(disabledSecurityProviderSource, securityOperationResolver, authorizationSystem, securityExpressionBuilderFactory)
        {
        }

        protected override SecurityPath<Employee> GetSecurityPath()
        {
            return SecurityPath<Employee>.Create(v => v.BusinessUnit);
        }
    }
}
