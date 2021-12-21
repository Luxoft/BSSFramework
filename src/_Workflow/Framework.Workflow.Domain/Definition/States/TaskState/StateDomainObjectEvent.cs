using Framework.Persistent;

namespace Framework.Workflow.Domain.Definition
{

    /// <summary>
    /// Cобытие изменения доменного объекта
    /// </summary>
    /// <remarks>
    /// Пример:
    /// (authContext, wfObj) => !authContext.Logics.Permission.IsTerminate(wfObj.DomainObject) - сигнатура лямбды "Terminate Lambdas"
    /// Лямбда "TerminateLambdas" эквивалентна сигнатуре лябды "StateDomainObjectEvent"
    /// Отличие только в том, что в первом случае возвращается boolean, а во втором - datetime
    /// </remarks>
    public class StateDomainObjectEvent : Event<State>, IDetail<State>
    {


        private WorkflowLambda condition;


        protected StateDomainObjectEvent()
        {

        }

        /// <summary>
        /// Конструктор создает событие изменения доменного объекта с ссылкой на состояние
        /// </summary>
        /// <param name="state">Состояние</param>
        public StateDomainObjectEvent(State state)
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