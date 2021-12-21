using System.Collections.Generic;

using Framework.Persistent;

namespace Framework.Workflow.Domain.Definition
{
    /// <summary>
    /// Состояние объекта воркфлоу с задачами
    /// </summary>
    /// <remarks>
    /// Объект находится в состоянии, пока пользователь не выполнит хотя бы одну из задач или не произойдет один их двух событий: <see cref="StateDomainObjectEvent"/> , <see cref="StateTimeoutEvent"/>
    /// </remarks>
    public partial class State : StateBase, IMaster<Task>, IMaster<StateTimeoutEvent>, IMaster<StateDomainObjectEvent>
    {
        private readonly ICollection<Task> tasks = new List<Task>();

        private readonly ICollection<StateTimeoutEvent> timeoutEvents = new List<StateTimeoutEvent>();

        private readonly ICollection<StateDomainObjectEvent> domainObjectEvents = new List<StateDomainObjectEvent>();


        private bool isFinal;

        /// <summary>
        /// Конструктор создает состояние с типом "Main" и ссылкой на воркфлоу
        /// </summary>
        /// <param name="workflow"></param>
        public State(Workflow workflow)
            : base(workflow, StateType.Main)
        {
            workflow.AddDetail(this);
        }

        protected State()
        {
        }

        /// <summary>
        /// Признак того, что состояние является финальным для воркфлоу
        /// </summary>
        public virtual bool IsFinal
        {
            get { return this.isFinal; }
            set { this.isFinal = value; }
        }

        /// <summary>
        /// Коллекция задач, привязанных к состоянию
        /// </summary>
        public virtual IEnumerable<Task> Tasks
        {
            get { return this.tasks; }
        }

        /// <summary>
        /// Коллекция событий истечения времени, отведённого для выполнения задач
        /// </summary>
        public virtual IEnumerable<StateTimeoutEvent> TimeoutEvents
        {
            get { return this.timeoutEvents; }
        }

        /// <summary>
        /// Коллекция событий изменения доменного объекта
        /// </summary>
        public virtual IEnumerable<StateDomainObjectEvent> DomainObjectEvents
        {
            get { return this.domainObjectEvents; }
        }


        #region IMaster<Task> Members

        ICollection<Task> IMaster<Task>.Details
        {
            get { return (ICollection<Task>)this.Tasks; }
        }

        #endregion

        #region IMaster Members

        ICollection<StateTimeoutEvent> IMaster<StateTimeoutEvent>.Details
        {
            get { return (ICollection<StateTimeoutEvent>)this.TimeoutEvents; }
        }

        ICollection<StateDomainObjectEvent> IMaster<StateDomainObjectEvent>.Details
        {
            get { return (ICollection<StateDomainObjectEvent>)this.DomainObjectEvents; }
        }

        #endregion
    }
}