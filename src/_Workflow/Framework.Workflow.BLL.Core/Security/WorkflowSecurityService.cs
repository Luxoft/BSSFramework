using System;
using System.Linq.Expressions;

using Framework.SecuritySystem;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.BLL
{
    public partial class WorkflowSecurityService
    {
        public ISecurityProvider<TDomainObject> GetWatcherSecurityProvider<TDomainObject>(Expression<Func<TDomainObject, WorkflowInstance>> path)
            where TDomainObject : PersistentDomainObjectBase
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            return new WorkflowInstanceWatcherSecurityProvider<TDomainObject>(this.Context, path);
        }
    }
}
