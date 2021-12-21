using System;

using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.BLL
{
    public class ExecutedCommandValidator : ParameterizedObjectValidator<Domain.Runtime.ExecutedCommand, Domain.Definition.Command, Domain.Runtime.ExecutedCommandParameter, Domain.Definition.CommandParameter>
    {
        public ExecutedCommandValidator(IWorkflowBLLContext context, ITargetSystemService targetSystemService)
            : base(context, targetSystemService)
        {

        }


        protected override object CreateParameterizedObject(ExecutedCommand executedCommand)
        {
            if (executedCommand == null) throw new ArgumentNullException(nameof(executedCommand));

            return this.TargetSystemService.GetAnonymousObject(executedCommand);
        }
    }
}