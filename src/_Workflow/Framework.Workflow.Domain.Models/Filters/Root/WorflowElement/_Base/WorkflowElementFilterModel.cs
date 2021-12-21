using System;
using System.Linq.Expressions;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.Domain
{
    public abstract class WorkflowElementFilterModel<TDomainObject> : DomainObjectFilterModel<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase, IWorkflowElement
    {
        public Workflow.Domain.Definition.Workflow Workflow { get; set; }


        public override Expression<Func<TDomainObject, bool>> ToFilterExpression()
        {
            var workflow = this.Workflow;

            return domainObject => workflow == null || domainObject.Workflow == workflow;
        }
    }
}