using Framework.DomainDriven;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Workflow.Domain.Definition
{

    /// <summary>
    /// Контейнер для одного события
    /// </summary>
    /// <typeparam name="TEvent">Тип события</typeparam>
    public abstract class SingleEventContainerBase<TEvent> : EventContainerBase<TEvent>
        where TEvent : Event
    {
        protected SingleEventContainerBase()
        {

        }

        /// <summary>
        /// Тип события
        /// </summary>
        [Required]
        [DetailRole(true)]
        [FetchPath("InternalEvents")]
        [WorkflowElementValidator]
        public virtual TEvent Event
        {
            get { return this.GetOneToOne(v => v.InternalEvents) ?? this.CreateDefaultEvent(); }
            set
            {
                if (value == null) { return; }

                var newValue = value;

                this.SetValueSafe(v => v.Event, newValue, () => this.SetOneToOne(v => v.InternalEvents, newValue));
            }
        }


        protected abstract TEvent CreateDefaultEvent();
    }
}