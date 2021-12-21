using System;
using System.Collections.Generic;

using Framework.Core;
using Framework.Workflow.Domain.Definition;
using Framework.Workflow.Domain.Runtime;

using JetBrains.Annotations;

namespace Framework.Workflow.BLL
{
    public interface IWorkflowMachine
    {
        WorkflowInstance WorkflowInstance
        {
            get;
        }


        WorkflowProcessResult ProcessTransition([NotNull]Transition transition, WorkflowProcessSettings processSettings);

        WorkflowProcessResult ProcessCurrentState(WorkflowProcessSettings processSettings);

        WorkflowProcessResult ProcessCurrentStateEvent(WorkflowProcessSettings processSettings);

        WorkflowProcessResult ProcessEvent([NotNull]Event @event, WorkflowProcessSettings processSettings);


        WorkflowProcessResult TryFinishParallel();


        StateInstance SwitchState([NotNull] StateBase newState);

        Event GetCurrentStateEvent();


        void Save();

        void Abort();



        void ExecuteCommandAction([NotNull]ExecutedCommand executedCommand);




        bool GetConditionResult([NotNull]ConditionState conditionState);

        bool GetParallelStateFinalEventResult([NotNull]ParallelStateFinalEvent parallelStateFinalEvent);


        bool IsTimeout([NotNull]StateTimeoutEvent stateTimeoutEvent, DateTime checkDate);

        bool IsEvaluated([NotNull]StateDomainObjectEvent stateDomainObjectEvent);



        bool HasAccess([NotNull]Task task);

        bool HasAccess([NotNull]Command command);


        UnboundedList<string> GetAccessors([NotNull]Task task);

        UnboundedList<string> GetAccessors([NotNull]Command command);


        bool TryChangeActive();


        IEnumerable<string> GetReversePath([NotNull] WorkflowSource workflowSource);
    }
}
