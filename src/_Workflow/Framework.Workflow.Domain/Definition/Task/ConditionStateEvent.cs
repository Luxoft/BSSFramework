using Framework.DomainDriven.Serialization;
using Framework.Persistent;

namespace Framework.Workflow.Domain.Definition
{

    /// <summary>
    /// Событие промежуточного условного состояния, при выполнении которого объект переходит в новое состояние
    /// </summary>
    /// <remarks>
    /// При инициализации условного состояния создаются 2-ва события с признаками «true» и «false», которые наследуется от события (event) и привязаны к переходу (transition)
    /// Таким образом, в условном состоянии определяется какой переход нужно выполнить
    /// В качестве входных параметров используются BLL и Workflow контексты
    /// </remarks>
    public class ConditionStateEvent : Event<ConditionState>
    {
        private bool isTrue;


        protected ConditionStateEvent()
        {

        }

        /// <summary>
        /// Конструктор создает событие промежуточного условного состояния с ссылкой на событие истечения времени
        /// </summary>
        /// <param name="stateTimeout">StateTimeoutEvent</param>
        public ConditionStateEvent(ConditionState stateTimeout)
            : base (stateTimeout)
        {
            stateTimeout.AddDetail(this);
        }

        /// <summary>
        /// Признак того, что событие положительно
        /// </summary>
        /// <remarks>
        /// Событие условного состояния
        /// </remarks>
        public virtual bool IsTrue
        {
            get { return this.isTrue; }
            internal protected set { this.isTrue = value; this.RecalcName(); }
        }

        /// <summary>
        /// Название события промежуточного условного состояния
        /// </summary>
        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        public override string Name
        {
            get { return base.Name; }
        }

        /// <summary>
        /// Состояние, к которому относится событие
        /// </summary>
        [ExpandPath("Owner")]
        public override StateBase SourceState
        {
            get { return this.Owner; }
        }

        /// <summary>
        /// Возвращает новое имя
        /// </summary>
        /// <returns>Новое имя</returns>
        protected override string GetNewName()
        {
            return base.GetNewName() + "_" + this.IsTrue;
        }
    }
}