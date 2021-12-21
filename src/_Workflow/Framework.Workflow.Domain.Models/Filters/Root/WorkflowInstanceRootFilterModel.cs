using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Framework.Core;

namespace Framework.Workflow.Domain
{
    public class WorkflowInstanceRootFilterModel : DomainObjectMultiFilterModel<Framework.Workflow.Domain.Runtime.WorkflowInstance>
    {
        public bool? IsFinal { get; set; }

        public bool? IsActive { get; set; }

        public Guid DomainObjectId { get; set; }

        public Workflow.Domain.Definition.Workflow Definition { get; set; }

        public Workflow.Domain.Definition.DomainType DomainType { get; set; }


        public WorkflowInstanceRootFilterModel()
        {

        }


        protected override IEnumerable<Expression<Func<Runtime.WorkflowInstance, bool>>> ToFilterExpressionItems()
        {
            if (!this.DomainObjectId.IsDefault())
            {
                var domainObjectId = this.DomainObjectId;

                yield return workflowInstance => workflowInstance.DomainObjectId == domainObjectId;
            }

            if (this.IsFinal.HasValue)
            {
                var isFinal = this.IsFinal.Value;

                yield return workflowInstance => workflowInstance.IsFinished == isFinal;
            }

            if (this.IsActive.HasValue)
            {
                var isActive = this.IsActive.Value;

                yield return workflowInstance => workflowInstance.Active == isActive;
            }

            if (this.Definition != null)
            {
                yield return workflowInstance => workflowInstance.Definition == this.Definition;
            }

            if (this.DomainType != null)
            {
                yield return workflowInstance => workflowInstance.Definition.DomainType == this.DomainType;
            }
        }
    }
}