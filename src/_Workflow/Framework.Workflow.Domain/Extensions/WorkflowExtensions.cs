using System;
using Framework.Core;
using Framework.Exceptions;

namespace Framework.Workflow.Domain.Definition
{

    public static class WorkflowExtensions
    {

        /// <summary>
        /// Возвращает переход, у которого TriggerEvent является заданное событие
        /// </summary>
        /// <param name="workflow">Воркфлоу</param>
        /// <param name="event">Событие</param>
        /// <returns>Переход</returns>
        public static Transition GetEventTransition(this Workflow workflow, Event @event)
        {
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));
            if (@event == null) throw new ArgumentNullException(nameof(@event));


            return workflow.Transitions.Single(tr => tr.TriggerEvent == @event,
                                               () => new BusinessLogicException($"transition for event \"{@event.Name}\" not found"),
                                               () => new BusinessLogicException($"To many transitions for event \"{@event.Name}\""));
        }
    }
}