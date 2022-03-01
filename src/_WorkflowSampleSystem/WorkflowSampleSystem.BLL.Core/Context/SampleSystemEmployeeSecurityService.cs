using System;

using Framework.SecuritySystem;
using Framework.SecuritySystem.Rules.Builders;

using JetBrains.Annotations;

using WorkflowSampleSystem.Domain;

namespace WorkflowSampleSystem.BLL
{
    public partial class WorkflowSampleSystemEmployeeSecurityService<TDomainObject, TBusinessUnit, TDepartment, TLocation, TEmployee>
    {
        public WorkflowSampleSystemEmployeeSecurityService(
            [NotNull] IAccessDeniedExceptionService<PersistentDomainObjectBase> accessDeniedExceptionService,
            [NotNull] IDisabledSecurityProviderContainer<PersistentDomainObjectBase> disabledSecurityProviderContainer,
            [NotNull] ISecurityOperationResolver<PersistentDomainObjectBase, WorkflowSampleSystemSecurityOperationCode> securityOperationResolver,
            [NotNull] IAuthorizationSystem<Guid> authorizationSystem,
            [NotNull] ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> securityExpressionBuilderFactory,
            [NotNull] IWorkflowSampleSystemSecurityPathContainer securityPathContainer,
            [NotNull] IWorkflowSampleSystemBLLContext context)

            : base(accessDeniedExceptionService, disabledSecurityProviderContainer, securityOperationResolver, authorizationSystem, securityExpressionBuilderFactory)
        {
            this.securityPathContainer = securityPathContainer ?? throw new ArgumentNullException(nameof(securityPathContainer));
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IWorkflowSampleSystemBLLContext Context { get; }


        protected override ISecurityProvider<TDomainObject> CreateSecurityProvider(ContextSecurityOperation<WorkflowSampleSystemSecurityOperationCode> securityOperation)
        {
            var baseProvider = base.CreateSecurityProvider(securityOperation);

            if (securityOperation == WorkflowSampleSystemSecurityOperation.EmployeeView)
            {
                return baseProvider.Or(employee => employee.Login == this.Context.Authorization.RunAsManager.PrincipalName, this.AccessDeniedExceptionService);
            }
            else
            {
                return baseProvider;
            }
        }
    }
}
