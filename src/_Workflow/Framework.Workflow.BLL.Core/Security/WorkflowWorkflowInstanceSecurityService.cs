using System;

using Framework.SecuritySystem;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Runtime;

using JetBrains.Annotations;

namespace Framework.Workflow.BLL
{
    public partial class WorkflowWorkflowInstanceSecurityService
    {
        public WorkflowWorkflowInstanceSecurityService(IAccessDeniedExceptionService<PersistentDomainObjectBase> accessDeniedExceptionService, IDisabledSecurityProviderContainer<PersistentDomainObjectBase> disabledSecurityProviderContainer, ISecurityOperationResolver<PersistentDomainObjectBase, WorkflowSecurityOperationCode> securityOperationResolver, IAuthorizationSystem<Guid> authorizationSystem,
                                                       [NotNull] IWorkflowBLLContext context)
            : base(accessDeniedExceptionService, disabledSecurityProviderContainer, securityOperationResolver, authorizationSystem)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IWorkflowBLLContext Context { get; }

        protected override ISecurityProvider<WorkflowInstance> CreateSecurityProvider(SecurityOperation<WorkflowSecurityOperationCode> securityOperation)
        {
            if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

            var baseProvider = base.CreateSecurityProvider(securityOperation);

            var watcherProvider = this.Context.SecurityService.GetWatcherSecurityProvider<WorkflowInstance>(workflowInstance => workflowInstance);

            if (securityOperation == WorkflowSecurityOperation.WorkflowView)
            {
                return baseProvider.Or(watcherProvider, this.AccessDeniedExceptionService);
            }
            else
            {
                return baseProvider;
            }
        }
    }
}
