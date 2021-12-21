using System;
using System.Linq;

using Framework.Core;
using Framework.Exceptions;
using Framework.Validation;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.BLL
{
    public class ExecuteCommandEventProcessor : WorfklowEventProcessor<ExecuteCommandRequest>
    {
        public ExecuteCommandEventProcessor(IWorkflowBLLContext context) : base(context)
        {

        }


        public override Event GetEvent(ExecuteCommandRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            this.Context.Validator.Validate(request);

            var taskInstance = request.TaskInstance;
            var workflowInstance = taskInstance.Workflow;
            var command = request.Command;

            if (workflowInstance.IsFinished)
            {
                throw new BusinessLogicException("WorkflowInstance \"{0}\" already finished", workflowInstance.Name);
            }

            if (!workflowInstance.Active)
            {
                throw new BusinessLogicException("WorkflowInstance \"{0}\" is disabled", workflowInstance.Name);
            }

            if (workflowInstance.CurrentState != taskInstance.State || !taskInstance.Definition.Commands.Contains(command))
            {
                throw new BusinessLogicException($"Command \"{command.Name}\" can't be executed in \"{workflowInstance.CurrentState.Definition.Name}\" state");
            }

            var targetSystemService = this.Context.GetTargetSystemService(command);
            var workflowMachine = targetSystemService.GetWorkflowMachine(workflowInstance);

            if (!targetSystemService.CommandAccessService.HasAccess(command, workflowInstance))
            {
                throw new BusinessLogicException("You have no access to \"{0}\" command", request.Command.Name);
            }

            var executedCommand = new ExecutedCommand(taskInstance) { Definition = request.Command };

            foreach (var requestParameter in request.Parameters)
            {
                if (!requestParameter.Definition.IsReadOnly)
                {
                    new ExecutedCommandParameter(executedCommand)
                    {
                        Definition = requestParameter.Definition,
                        Value = requestParameter.Value
                    };
                }
            }

            //{
            //    var accessors = workflowMachine.GetAccessors(request.Command);

            //    var executedCommandParameterDefinition = this.Context.Logics.Command.GetPotentialApproversParameter(command);

            //    if (executedCommandParameterDefinition.IsNew)
            //    {
            //        this.Context.Logics.Default.Create<CommandParameter>().Save(executedCommandParameterDefinition);
            //    }

            //    this.Context.Validator.Validate(command);

            //    new ExecutedCommandParameter(executedCommand)
            //    {
            //        Definition = executedCommandParameterDefinition,
            //        Value = accessors.IsInfinity ? "[ALL USERS]" : accessors.Join(", ")
            //    };
            //}

            this.Context.Logics.ExecutedCommand.Save(executedCommand);

            if (command.ExecuteAction != null)
            {
                workflowMachine.ExecuteCommandAction(executedCommand);
            }

            return command.Event;
        }
    }
}