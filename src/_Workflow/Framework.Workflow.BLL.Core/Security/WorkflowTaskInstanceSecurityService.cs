using System;
using Framework.SecuritySystem;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Runtime;

using JetBrains.Annotations;

namespace Framework.Workflow.BLL
{
    public partial class WorkflowTaskInstanceSecurityService
    {
        public WorkflowTaskInstanceSecurityService(IAccessDeniedExceptionService<PersistentDomainObjectBase> accessDeniedExceptionService, IDisabledSecurityProviderContainer<PersistentDomainObjectBase> disabledSecurityProviderContainer, ISecurityOperationResolver<PersistentDomainObjectBase, WorkflowSecurityOperationCode> securityOperationResolver, IAuthorizationSystem<Guid> authorizationSystem, [NotNull] IWorkflowBLLContext context)
            : base(accessDeniedExceptionService, disabledSecurityProviderContainer, securityOperationResolver, authorizationSystem)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public IWorkflowBLLContext Context { get; }


        protected override ISecurityProvider<TaskInstance> CreateSecurityProvider(SecurityOperation<WorkflowSecurityOperationCode> securityOperation)
        {
            if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

            var baseProvider = base.CreateSecurityProvider(securityOperation);

            if (securityOperation == WorkflowSecurityOperation.WorkflowView)
            {
                var watcherProvider = this.Context.SecurityService.GetWatcherSecurityProvider<TaskInstance>(taskInstance => taskInstance.Workflow);

                //var assineeProvider = this.Context.SecurityService.GetAssigneeSecurityProvider<TaskInstance>(taskInstance => taskInstance);

                //var mainSecurityProvider = new TaskInstanceMainSecurityProvider(this.Context);

                return baseProvider.Or(watcherProvider, this.AccessDeniedExceptionService)
                                 //.Or(assineeProvider)
                                  // .Or(mainSecurityProvider)
                                   ;
            }
            else
            {
                return baseProvider;
            }
        }
    }
}
