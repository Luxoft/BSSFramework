using System;
using System.Linq;

using Framework.Workflow.Domain.Definition;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.BLL
{
    public interface ICommandAccessService
    {
        bool HasAccess(Command command, WorkflowInstance workflowInstance);
    }

    public class CommandAccessService : ICommandAccessService
    {
        private readonly ITargetSystemService _targetSystemService;

        public CommandAccessService(ITargetSystemService targetSystemService)
        {
            if (targetSystemService == null) throw new ArgumentNullException(nameof(targetSystemService));

            this._targetSystemService = targetSystemService;
        }


        public bool HasAccess(Command command, WorkflowInstance workflowInstance)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (workflowInstance == null) throw new ArgumentNullException(nameof(workflowInstance));
            if (workflowInstance.Definition != command.Workflow)  throw new ArgumentException("Different workflow");

            if (!command.RoleAccesses.Any() || !workflowInstance.IsAvailable)
            {
                return false;
            }

            return this._targetSystemService.GetWorkflowMachine(workflowInstance).HasAccess(command);
        }
    }
}