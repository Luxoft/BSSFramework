using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Workflow.Domain.Definition;
using Framework.Workflow.Domain.Runtime;

namespace Framework.Workflow.BLL
{
    public class StateInstanceEventProcessor : WorfklowMachineEventProcessor<StateInstance>
    {
        public StateInstanceEventProcessor(IWorkflowBLLContext context, IWorkflowMachine workflowMachine)
            : base(context, workflowMachine)
        {

        }

        public override Event GetEvent(StateInstance stateInstance)
        {
            if (stateInstance == null) throw new ArgumentNullException(nameof(stateInstance));

            return this.GetEvents(stateInstance).FirstOrDefault();
        }


        private IEnumerable<Event> GetEvents(StateInstance stateInstance)
        {
            if (stateInstance == null) throw new ArgumentNullException(nameof(stateInstance));

            var state = this.Context.GetNestedStateBase(stateInstance.Definition);

            if (state is State)
            {
                var dateTime = this.Context.DateTimeService.Now;

                var timeoutEvents = from timeoutEvent in ((State)state).TimeoutEvents

                                    where this.WorkflowMachine.IsTimeout(timeoutEvent, dateTime)

                                    select timeoutEvent;

                foreach (var @event in timeoutEvents)
                {
                    yield return @event;
                }

                var domainObjectEvents = from domainObjectEvent in ((State)state).DomainObjectEvents

                                         where this.WorkflowMachine.IsEvaluated(domainObjectEvent)

                                         select domainObjectEvent;

                foreach (var @event in domainObjectEvents)
                {
                    yield return @event;
                }
            }
            else if (state is ConditionState)
            {
                var conditionState = (ConditionState)state;

                var condResult = this.WorkflowMachine.GetConditionResult(conditionState);

                var @event = condResult ? conditionState.TrueEvent : conditionState.FalseEvent;

                yield return @event;
            }
            else if (state is ParallelState)
            {
                var parallelState = (ParallelState)state;

                var events = from finalEvent in parallelState.FinalEvents

                             orderby finalEvent.OrderIndex

                             where this.WorkflowMachine.GetParallelStateFinalEventResult(finalEvent)

                             select finalEvent;


                foreach (var @event in events)
                {
                    yield return @event;
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(stateInstance));
            }
        }
    }
}