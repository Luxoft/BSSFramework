using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.Core.Serialization;
using Framework.DomainDriven.BLL;
using Framework.Exceptions;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.Validation;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;

using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.BLL
{
    public partial class WorkflowInstanceBLL
    {
        public override void Save(WorkflowInstance workflowInstance)
        {
            if (workflowInstance == null) throw new ArgumentNullException(nameof(workflowInstance));

            this.Recalculate(workflowInstance);

            base.Save(workflowInstance);
        }

        private void Recalculate(WorkflowInstance workflowInstance)
        {
            if (workflowInstance == null) throw new ArgumentNullException(nameof(workflowInstance));

            if (workflowInstance.Owner == null)
            {
                workflowInstance.GetAllChildren(true).Where(wi => wi.Active != workflowInstance.Active).Foreach(wi =>
                {
                    wi.Active = workflowInstance.Active;
                    base.Save(wi);
                });
            }
            else
            {
                workflowInstance.Active = workflowInstance.Owner.Active;
            }

            this.RecalculateParameters(workflowInstance);
        }

        private void RecalculateParameters(WorkflowInstance workflowInstance)
        {
            if (workflowInstance == null) throw new ArgumentNullException(nameof(workflowInstance));

            var domainObjectIdRequest = from domainObjectParameter in workflowInstance.DomainObjectParameter.ToMaybe()

                                        from result in ParserHelper.TryParse<Guid>(domainObjectParameter.Value)

                                        select result;

            workflowInstance.DomainObjectId = domainObjectIdRequest.GetValueOrDefault();

            workflowInstance.Name = workflowInstance.GetParameter(WorkflowParameterRole.InstanceIdentity)
                            .Maybe(instanceIdentityParameter => instanceIdentityParameter.Value, workflowInstance.Definition.Name);

            workflowInstance.Description = workflowInstance.GetParameter(WorkflowParameterRole.InstanceDescription)
                            .Maybe(instanceDescriptionParameter => instanceDescriptionParameter.Value, workflowInstance.Definition.Description);
        }

        protected override void Validate(WorkflowInstance workflowInstance, WorkflowOperationContext context)
        {
            base.Validate(workflowInstance, context);

            new WorkflowInstanceValidator(this.Context, this.Context.GetTargetSystemService(workflowInstance)).GetValidateResult(workflowInstance).TryThrow();
        }

        public WorkflowProcessResult TryExecuteCommands(Guid domainObjectId, Command command, Dictionary<string, string> parameters)
        {
            if (domainObjectId.IsDefault()) throw new ArgumentOutOfRangeException(nameof(domainObjectId));
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            var availableTaskWorkflowGroups = this.Context.Logics.TaskInstance.GetAvailableGroups(new AvailableTaskInstanceMainFilterModel { SourceType = command.Workflow.DomainType, DomainObjectId = domainObjectId });

            var taskInstancesRequest = from availableTaskWorkflowGroup in availableTaskWorkflowGroups

                                       where availableTaskWorkflowGroup.Workflow == command.Workflow

                                       from availableTaskGroup in availableTaskWorkflowGroup.Items

                                       from requestItem in availableTaskGroup.Items

                                       where requestItem.Commands.Contains(command)

                                       from taskInstance in requestItem.TaskInstances

                                       where taskInstance.IsAvailable

                                       select taskInstance;

            var taskInstances = taskInstancesRequest.ToList();

            if (!taskInstances.Any())
            {
                throw new BusinessLogicException($"Available tasks by workflow \"{command.Workflow.Name}\") for domainObject \"{domainObjectId}\" not found");
            }

            var preResult = this.ExecuteCommands(new MassExecuteCommandRequest
            {
                Groups = new[]
                {
                    new GroupExecuteCommandRequest
                    {
                        Command = command,
                        Parameters = command.ToExecuteCommandRequestParameters(parameters).ToList(),
                        TaskInstances = taskInstances
                    }
                }
            });

            return preResult;
        }

        public WorkflowProcessResult ExecuteCommands(MassExecuteCommandRequest massRequest)
        {
            if (massRequest == null) throw new ArgumentNullException(nameof(massRequest));

            this.Context.Validator.Validate(massRequest);

            var eventProcessor = new ExecuteCommandEventProcessor(this.Context);

            var resultRequest = from singleRequest in massRequest.Split()

                                let workflowMachine = this.Context.GetWorkflowMachine(singleRequest.TaskInstance.Workflow)

                                let @event = eventProcessor.GetEvent(singleRequest)

                                select workflowMachine.ProcessEvent(@event, WorkflowProcessSettings.SkipTryFinishParallel);

            var preResult = resultRequest.Sum();

            return this.Context.FinishParallels(preResult);
        }

        public WorkflowProcessResult ExecuteCommand(ExecuteCommandRequest singleRequest)
        {
            if (singleRequest == null) throw new ArgumentNullException(nameof(singleRequest));

            this.Context.Validator.Validate(singleRequest);

            var eventProcessor = new ExecuteCommandEventProcessor(this.Context);

            var @event = eventProcessor.GetEvent(singleRequest);

            return this.Context.GetWorkflowMachine(singleRequest.TaskInstance.Workflow).ProcessEvent(@event, WorkflowProcessSettings.Default);
        }


        public WorkflowInstance Start(StartWorkflowRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            this.Context.Validator.Validate(request);

            var workflowMachine = this.Context.StartWorkflowMachine(request);

            workflowMachine.ProcessCurrentState(WorkflowProcessSettings.Default);

            return workflowMachine.WorkflowInstance;
        }

        public void Abort(WorkflowInstance workflowInstance)
        {
            if (workflowInstance == null) throw new ArgumentNullException(nameof(workflowInstance));

            this.Context.GetWorkflowMachine (workflowInstance).Abort();
        }

        public override void Remove(WorkflowInstance workflowInstance)
        {
            if (workflowInstance == null) throw new ArgumentNullException(nameof(workflowInstance));

            if (workflowInstance.Owner != null)
            {
                throw new BusinessLogicException("Can't remove child subworkflow");
            }

            base.Remove(workflowInstance);
        }


        public ITryResult<WorkflowProcessResult>[] CheckTimeouts()
        {
            var wfInstanceQ = this.GetUnsecureQueryable(rule => rule.Select(wfInstance => wfInstance.CurrentState)
                                                            .Select(wfInstance => wfInstance.Parameters)
                                                            .SelectNested(wfInstance => wfInstance.OwnerState)
                                                            .SelectNested(wfInstance => wfInstance.Workflow)
                                                            .Select(wfInstance => wfInstance.Parameters));

            var stateQ = this.Context.Logics.State.GetUnsecureQueryable();

            var potentialTimeoutsRequest = from wfInstance in wfInstanceQ

                                           where wfInstance.Active

                                           let currentState = wfInstance.CurrentState

                                           join state in stateQ on currentState.Definition equals state

                                           where state.TimeoutEvents.Any()

                                           select wfInstance;

            var potentialTimeouts = potentialTimeoutsRequest.ToList();


            var potentialTimeoutGroups = potentialTimeouts.GroupBy(pair => pair.Definition)
                                                          .ToList();

            return potentialTimeoutGroups.SelectMany(potentialTimeoutGroup => this.Context.GetMassWorkflowMachine(potentialTimeoutGroup.Key, potentialTimeoutGroup.ToArray()).ProcessTimeouts())
                                         .ToArray();
        }
    }
}
