using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven;
using Framework.Exceptions;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Workflow.Domain.Definition
{

    /// <summary>
    /// Промежуточное условное состояние, при выполнении которого объект переходит в новое состояние
    /// </summary>
    /// <remarks>
    /// При инициализации условного состояния создаются 2-ва события с признаками «true» и «false», которые наследуется от события (event) и привязаны к переходу (transition)
    /// Таким образом, в условном состоянии определяется какой переход нужно выполнить
    /// В качестве входных параметров используются BLL и Workflow контексты
    /// </remarks>
    public class ConditionState : StateBase, IMaster<ConditionStateEvent>
    {
        private readonly ICollection<ConditionStateEvent> internalEvents = new List<ConditionStateEvent>();

        private WorkflowLambda condition;

        /// <summary>
        /// Коструктор создает условное состояние с ссылкой на воркфлоу
        /// </summary>
        /// <param name="workflow">Воркфлоу</param>
        public ConditionState(Workflow workflow)
            : base(workflow, StateType.Condition)
        {
            workflow.AddDetail(this);
        }

        protected ConditionState()
        {
        }

       /// <summary>
       /// Название условного состояния
       /// </summary>
        public override string Name
        {
            get { return base.Name; }
            set
            {
                base.Name = value;
                this.InternalEvents.Foreach(e => e.RecalcName());
            }
        }

        /// <summary>
        /// Вычисляемое отрицательное (false) условное событие
        /// </summary>
        [Required]
        [DetailRole(true)]
        [FetchPath("InternalEvents")]
        [WorkflowElementValidator]
        public virtual ConditionStateEvent FalseEvent
        {
            get { return this.GetEvent(false); }
            set { this.SetEvent(false, value); }
        }

        /// <summary>
        /// Вычисляемое положительное (true) условное событие
        /// </summary>
        [Required]
        [DetailRole(true)]
        [FetchPath("InternalEvents")]
        [WorkflowElementValidator]
        public virtual ConditionStateEvent TrueEvent
        {
            get { return this.GetEvent(true); }
            set { this.SetEvent(true, value); }
        }

        /// <summary>
        /// Лямбда-выражение условного события, которое привязано к состоянию
        /// </summary>
        /// <remarks>
        /// На основании условного события будет выбран результат состояния, в который перейдет объект
        /// </remarks>
        [RequiredStartValidator]
        [WorkflowElementValidator]
        public virtual WorkflowLambda Condition
        {
            get { return this.condition; }
            set { this.condition = value; }
        }

        /// <summary>
        /// Коллекция событий условного состояния
        /// </summary>
        /// <remarks>
        /// События с положительным/отрицательным выходом
        /// </remarks>
        internal protected virtual IEnumerable<ConditionStateEvent> InternalEvents
        {
            get { return this.internalEvents; }
        }

        private ConditionStateEvent GetEvent(bool isTrue)
        {
            var @event = this.InternalEvents.SingleOrDefault(e => e.IsTrue == isTrue) ?? new ConditionStateEvent(this) { IsTrue = isTrue };

            return @event;
        }

        private void SetEvent(bool isTrue, ConditionStateEvent conditionStateEvent)
        {
            if (conditionStateEvent == null)
            {
                new ConditionStateEvent(this) { IsTrue = isTrue };
            }
            else
            {
                if (!this.InternalEvents.Contains(conditionStateEvent))
                {
                    throw new BusinessLogicException("InvalidEvent");
                }

                conditionStateEvent.IsTrue = isTrue;
            }
        }


        #region IMaster<ConditionStateEvent> Members

        ICollection<ConditionStateEvent> IMaster<ConditionStateEvent>.Details
        {
            get { return (ICollection<ConditionStateEvent>)this.InternalEvents; }
        }

        #endregion
    }
}