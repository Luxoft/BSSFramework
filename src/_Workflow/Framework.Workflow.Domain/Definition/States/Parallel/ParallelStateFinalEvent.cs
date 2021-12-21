using Framework.Persistent;

namespace Framework.Workflow.Domain.Definition
{

    /// <summary>
    /// Событие выхода из параллельного состояния, связанное с условием его исполнения
    /// </summary>
    /// <remarks>
    /// При выходе из параллельного состояния всегда проверяется все условия и создаются 2-ва события с признаками «true» и «false»
    /// </remarks>
    public class ParallelStateFinalEvent : Event<ParallelState>, IOrderObject<int>
    {
        private WorkflowLambda condition;

        private int orderIndex;


        protected ParallelStateFinalEvent()
        {

        }

        /// <summary>
        /// Конструктор создает событие выхода из параллельного состояния с ссылкой на параллельное состояние
        /// </summary>
        /// <param name="parallelState">Параллельное состояние</param>
        public ParallelStateFinalEvent(ParallelState parallelState)
            : base(parallelState)
        {
            parallelState.AddDetail(this);
        }

        /// <summary>
        /// Условие при выходе из параллельного состояния
        /// </summary>
        [RequiredStartValidator]
        [WorkflowElementValidator]
        public virtual WorkflowLambda Condition
        {
            get { return this.condition; }
            set { this.condition = value; }
        }

        /// <summary>
        /// Порядок проверки условий при выходе из параллельного состояния
        /// </summary>
        public virtual int OrderIndex
        {
            get { return this.orderIndex; }
            set { this.orderIndex = value; }
        }

        /// <summary>
        /// Состояние, к которому относится событие
        /// </summary>
        [ExpandPath("Owner")]
        public override StateBase SourceState
        {
            get { return this.Owner; }
        }
    }
}