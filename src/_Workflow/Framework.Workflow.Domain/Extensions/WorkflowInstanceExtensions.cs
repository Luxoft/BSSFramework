using System;
using Framework.Exceptions;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.Domain.Runtime
{

    public static class WorkflowInstanceExtensions
    {

        /// <summary>
        /// Находит переход для definition экземпляра ворклоу, у которого TriggerEvent является заданное событие
        /// Смотрит, чтобы этот переход был не из того состояния, в котором сейчас находится экземпляр ворклоу
        /// </summary>
        /// <param name="workflowInsntance">Экземпляр ворклос</param>
        /// <param name="event">Событие</param>
        /// <returns>Переход</returns>
        public static Transition GetEventTransition(this WorkflowInstance workflowInsntance, Event @event)
        {
            if (workflowInsntance == null) throw new ArgumentNullException(nameof(workflowInsntance));
            if (@event == null) throw new ArgumentNullException(nameof(@event));

            var transition = workflowInsntance.Definition.GetEventTransition(@event);

            if (transition.From != workflowInsntance.CurrentState.Definition)
            {
                throw new BusinessLogicException("Invalid transition source");
            }

            return transition;
        }
    }
}