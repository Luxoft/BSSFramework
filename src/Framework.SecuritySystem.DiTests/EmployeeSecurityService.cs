using System;

using Framework.SecuritySystem.Rules.Builders;

using JetBrains.Annotations;

namespace Framework.SecuritySystem.DiTests
{
    public class EmployeeSecurityService : ContextDomainSecurityService<PersistentDomainObjectBase, Employee, Guid, ExampleSecurityOperation>
    {
        public EmployeeSecurityService(
            [NotNull] IDisabledSecurityProviderContainer<PersistentDomainObjectBase> disabledSecurityProviderContainer,
            [NotNull] ISecurityOperationResolver<PersistentDomainObjectBase, ExampleSecurityOperation> securityOperationResolver,
            [NotNull] IAuthorizationSystem<Guid> authorizationSystem,
            [NotNull] ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> securityExpressionBuilderFactory)

            : base(accessDeniedExceptionService, disabledSecurityProviderContainer, securityOperationResolver, authorizationSystem, securityExpressionBuilderFactory)
        {
        }

        protected override SecurityPathBase<PersistentDomainObjectBase, Employee, Guid> GetSecurityPath()
        {
            return SecurityPath<PersistentDomainObjectBase, Employee, Guid>.Create(v => v.BusinessUnit);
        }
    }
}
