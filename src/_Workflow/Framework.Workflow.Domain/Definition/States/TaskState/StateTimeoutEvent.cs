using Framework.Persistent;

namespace Framework.Workflow.Domain.Definition
{

    /// <summary>
    /// Событие истечения времени, отведённого для выполнения задач
    /// </summary>
    ///  <remarks>
    ///  Пример:
    ///  wfObj.OwnerWorkflow.DomainObject.Period.EndDateValue
    /// </remarks>
    public class StateTimeoutEvent : Event<State>, IDetail<State>
    {
        private WorkflowLambda condition;


        protected StateTimeoutEvent()
        {

        }

        /// <summary>
        /// Конструктор создает событие истечения времени, отведённого для выполнения задач, с ссылкой на состояние
        /// </summary>
        /// <param name="state">Состояние</param>
        public StateTimeoutEvent(State state)
            : base(state)
        {
            state.AddDetail(this);
        }

        /// <summary>
        /// Условие для события
        /// </summary>
        [RequiredStartValidator]
        [WorkflowElementValidator]
        public virtual WorkflowLambda Condition
        {
            get { return this.condition; }
            set { this.condition = value; }
        }

        /// <summary>
        /// Состояние, к которому относится событие
        /// </summary>
        [ExpandPath("Owner")]
        public override StateBase SourceState
        {
            get { return this.Owner; }
        }

        #region IDetail<StateTimeout> Members

        State IDetail<State>.Master
        {
            get { return this.Owner; }
        }

        #endregion
    }
}