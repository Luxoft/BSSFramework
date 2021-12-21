using System;

using Framework.Persistent;

namespace Framework.Workflow.Domain.Definition
{
    /// <summary>
    /// Действие на событие, которое производится с объектом после смены состояния воркфлоу
    /// </summary>
    /// <remarks>
    /// Переход может иметь несколько действий, ссылающихся на разные лямбды
    /// Переход может не иметь действий, тогда объект перейдёт в новое состояние без каких-либо сайд-эффектов
    /// Тип лямбды "Action" не возвращает никакого значения, а только принимает в себя БЛЛ и воркфлоу контексты:
    /// Из воркфлоу контекста вытаскивается сам доменный объект, который в данный момент изменяется
    /// Из БЛЛ контекста вытаскивается метод логики, который необходимо выполнить
    /// </remarks>
    [WorkflowViewDomainObject]
    [WorkflowEditDomainObject]
    public class TransitionAction : AuditPersistentDomainObjectBase, IDetail<Transition>, IWorkflowElement, IOrderObject<int>
    {
        private readonly Transition transition;

        private WorkflowLambda action;

        private int orderIndex;


        protected TransitionAction()
        {

        }

        /// <summary>
        /// Конструктор создает действие с ссылкой на переход
        /// </summary>
        /// <param name="transition">Переход</param>
        public TransitionAction(Transition transition)
        {
            if (transition == null) throw new ArgumentNullException(nameof(transition));

            this.transition = transition;
            this.transition.AddDetail(this);
        }

        /// <summary>
        /// Переход, к которому относится действие
        /// </summary>
        public virtual Transition Transition
        {
            get { return this.transition; }
        }

        /// <summary>
        /// Лямбда-выражение, которая выполняет действие
        /// </summary>
        [RequiredStartValidator]
        [WorkflowElementValidator]
        public virtual WorkflowLambda Action
        {
            get { return this.action; }
            set { this.action = value; }
        }

        /// <summary>
        /// Очередность действий в рамках одного перехода
        /// </summary>
        public virtual int OrderIndex
        {
            get { return this.orderIndex; }
            set { this.orderIndex = value; }
        }


        /// <summary>
        /// Воркфлоу, к которому относится действие
        /// </summary>
        [ExpandPath("Transition.Workflow")]
        public virtual Workflow Workflow
        {
            get { return this.Transition.Workflow; }
        }


        Transition IDetail<Transition>.Master
        {
            get { return this.Transition; }
        }
    }
}