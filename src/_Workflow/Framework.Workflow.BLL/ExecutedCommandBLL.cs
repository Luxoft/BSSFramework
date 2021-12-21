using System;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.BLL
{
    public partial class ExecutedCommandBLL
    {
        protected override void Validate(ExecutedCommand executedCommand, WorkflowOperationContext context)
        {
            if (executedCommand == null) throw new ArgumentNullException(nameof(executedCommand));

            new ExecutedCommandValidator(this.Context, this.Context.GetTargetSystemService(executedCommand)).GetValidateResult(executedCommand).TryThrow();
        }
    }
}