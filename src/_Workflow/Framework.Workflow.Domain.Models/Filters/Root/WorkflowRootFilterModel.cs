using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Framework.Workflow.Domain
{
    public class WorkflowRootFilterModel : DomainObjectMultiFilterModel<Framework.Workflow.Domain.Definition.Workflow>
    {
        [DefaultValue(true)]
        public bool? IsRoot { get; set; }


        public Definition.Workflow Owner { get; set; }


        protected override IEnumerable<Expression<Func<Definition.Workflow, bool>>> ToFilterExpressionItems()
        {
            if (this.IsRoot.HasValue)
            {
                var isRoot = this.IsRoot.Value;

                yield return workflow => isRoot == (workflow.Owner == null);
            }

            if (this.Owner != null)
            {
                yield return workflow => workflow.Owner == this.Owner;
            }
        }
    }
}