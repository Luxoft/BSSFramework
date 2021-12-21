using System;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Validation;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.BLL
{
    public partial class DomainBLLBase<TDomainObject, TOperation>
    {
        protected virtual void Validate(TDomainObject domainObject, WorkflowOperationContext context)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            this.Context.Validator.Validate(domainObject, (int)context);
        }

        public override void Save(TDomainObject domainObject)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            this.Validate(domainObject, WorkflowOperationContext.Save);

            base.Save(domainObject);
        }

        protected override System.Collections.Generic.IEnumerable<System.Linq.Expressions.ExpressionVisitor> GetVisitors()
        {
            yield return new OverrideCallInterfacePropertiesVisitor(typeof(IWorkflowElement));

            foreach (var baseVisitor in base.GetVisitors())
            {
                yield return baseVisitor;
            }
        }
    }
}