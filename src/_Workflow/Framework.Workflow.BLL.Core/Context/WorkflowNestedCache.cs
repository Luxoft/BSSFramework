using System;

using Framework.DomainDriven.BLL;
using Framework.Workflow.Domain;

namespace Framework.Workflow.BLL
{
    public class WorkflowNestedCache<TDomainObject> : NestedCache<IWorkflowBLLContext, PersistentDomainObjectBase, Guid, TDomainObject>
        where TDomainObject : PersistentDomainObjectBase
    {
        public WorkflowNestedCache(IWorkflowBLLContext context) : base(context)
        {

        }
    }
}