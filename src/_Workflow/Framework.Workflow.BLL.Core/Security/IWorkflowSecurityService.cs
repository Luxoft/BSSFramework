using System;
using System.Linq.Expressions;

using Framework.SecuritySystem;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.BLL
{
    public partial interface IWorkflowSecurityService
    {
        ISecurityProvider<TDomainObject> GetWatcherSecurityProvider<TDomainObject>(Expression<Func<TDomainObject, WorkflowInstance>> path)
            where TDomainObject : PersistentDomainObjectBase;
    }
}
