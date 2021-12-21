using System.Collections.Generic;
using Framework.Core;
using Framework.Persistent;

namespace Framework.Workflow.Domain.Definition
{

    /// <summary>
    /// Контейнер для хранения событий
    /// </summary>
    /// <typeparam name="TEvent">Тип события</typeparam>
    public abstract class EventContainerBase<TEvent> : WorkflowItemBase, IMaster<TEvent>, IWorkflowElement
        where TEvent : Event
    {
        protected readonly ICollection<TEvent> internalEvents = new List<TEvent>();


        protected EventContainerBase()
        {

        }

        /// <summary>
        /// Коллекция событий
        /// </summary>
        internal protected virtual IEnumerable<TEvent> InternalEvents
        {
            get { return this.internalEvents; }
        }

        /// <summary>
        /// Воркфлоу, к которому относится событие
        /// </summary>
        public abstract Workflow Workflow { get; }


        /// <summary>
        /// Название контейнера
        /// </summary>
        public override string Name
        {
            get { return base.Name; }
            set
            {
                base.Name = value;
                this.RecalcEventName();
            }
        }

        /// <summary>
        /// Метод агрегированных событий
        /// </summary>
        private void RecalcEventName()
        {
            this.InternalEvents.Foreach(e => e.RecalcName());
        }

        #region IMaster<TEvent> Members

        ICollection<TEvent> IMaster<TEvent>.Details
        {
            get { return (ICollection<TEvent>)this.InternalEvents; }
        }

        #endregion
    }
}