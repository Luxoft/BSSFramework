using System;
using System.Linq.Expressions;

using Framework.Core;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.Domain
{
    public class WorkflowSourceRootFilterModel : WorkflowElementFilterModel<WorkflowSource>
    {
        public DomainType WorkflowType { get; set; }

        public DomainType SourceType { get; set; }


        public override Expression<Func<WorkflowSource, bool>> ToFilterExpression()
        {
            var workflowType = this.WorkflowType;
            var sourceType = this.SourceType;

            return base.ToFilterExpression().BuildAnd(workflowSource => (workflowType == null || workflowType == workflowSource.WorkflowType)
                                                                     && (sourceType   == null || sourceType   == workflowSource.Type));
        }
    }
}