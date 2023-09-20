using Framework.SecuritySystem.Rules.Builders;

namespace Framework.SecuritySystem.DiTests
{
    public class EmployeeSecurityService : ContextDomainSecurityService<Employee, Guid>
    {
        public EmployeeSecurityService(
            IDisabledSecurityProviderSource disabledSecurityProviderSource,
            ISecurityOperationResolver securityOperationResolver,
            IAuthorizationSystem<Guid> authorizationSystem,
            ISecurityExpressionBuilderFactory securityExpressionBuilderFactory,
            SecurityPath<Employee> securityPath)

            : base(disabledSecurityProviderSource, securityOperationResolver, authorizationSystem, securityExpressionBuilderFactory, securityPath)
        {
        }
    }
}
