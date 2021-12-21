using System;
using System.Linq;

using Framework.Core;
using Framework.Exceptions;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Runtime;

using JetBrains.Annotations;

namespace Framework.Workflow.BLL
{
    public partial class WorkflowBLLContext
    {
        public IWorkflowMachine GetWorkflowMachine(WorkflowInstance workflowInstance)
        {
            if (workflowInstance == null) throw new ArgumentNullException(nameof(workflowInstance));

            return this.GetTargetSystemService(workflowInstance.Definition).GetWorkflowMachine(workflowInstance);
        }

        public IMassWorkflowMachine GetMassWorkflowMachine(Domain.Definition.Workflow definition, WorkflowInstance[] workflowInstances)
        {
            if (definition == null) throw new ArgumentNullException(nameof(definition));
            if (workflowInstances == null) throw new ArgumentNullException(nameof(workflowInstances));

            return this.GetTargetSystemService(definition).GetMassWorkflowMachine(definition, workflowInstances);
        }

        public WorkflowProcessResult FinishParallels([NotNull] WorkflowProcessResult processResult)
        {
            if (processResult == null) throw new ArgumentNullException(nameof(processResult));

            if (processResult.TryFinishParallelInstances.Any())
            {
                var finishResultRequest = from wi in processResult.TryFinishParallelInstances

                                          let machine = this.GetWorkflowMachine(wi)

                                          select machine.TryFinishParallel();

                var finishResult = finishResultRequest.Sum();

                return new WorkflowProcessResult(processResult.ExecutedTransitions.Concat(finishResult.ExecutedTransitions), finishResult.TryFinishParallelInstances);
            }
            else
            {
                return processResult;
            }
        }

        public IWorkflowMachine StartWorkflowMachine(StartWorkflowRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var workflow = request.Workflow;

            var workflowInstance = new WorkflowInstance(workflow, (request as StartWorkflowRequest.StartSubWorkflowRequest).Maybe(v => v.OwnerWorkflowState));

            foreach (var requestParameter in request.Parameters)
            {
                new WorkflowInstanceParameter(workflowInstance)
                {
                    Definition = requestParameter.Definition,
                    Value = requestParameter.Value
                };
            }

            var startState = (request as StartWorkflowRequest.CustomStateStartWorkflowRequest).Maybe(r => r.StartState)

                          ?? workflow.StateBases.Single(state => state.IsInitial, () => new BusinessLogicException("Initial state not found"),
                                                                                  () => new BusinessLogicException("More one initial state"));

            var machine = this.GetWorkflowMachine(workflowInstance);

            machine.SwitchState(startState);
            machine.Save();

            return machine;
        }
    }
}
