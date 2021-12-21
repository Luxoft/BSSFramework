using System;
using System.Linq;
using System.Linq.Expressions;
using Framework.Core;
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.BLL
{
    public class WorkflowInstanceWatcherSecurityProvider<TDomainObject> : SecurityProvider<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase
    {

        private readonly Expression<Func<TDomainObject, WorkflowInstance>> path;


        public WorkflowInstanceWatcherSecurityProvider(IWorkflowBLLContext context, Expression<Func<TDomainObject, WorkflowInstance>> path)
            : base(context.AccessDeniedExceptionService)
        {
            this.Context = context;
            this.path = path ?? throw new ArgumentNullException(nameof(path));
        }

        public IWorkflowBLLContext Context { get; }


        public override Expression<Func<TDomainObject, bool>> SecurityFilter
        {
            get
            {
                return from workflowInstance in this.path

                       select workflowInstance.Watchers.Any(watcher => watcher.Login == this.Context.Authorization.RunAsManager.PrincipalName);
            }
        }


        public override UnboundedList<string> GetAccessors(TDomainObject domainObject)
        {
            var workflowInstance = this.path.Eval(domainObject, this.CompileCache);

            return workflowInstance.Watchers.Select(watcher => watcher.Login).ToUnboundedList();
        }
    }
}
