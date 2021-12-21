using System;
using System.Collections.Generic;

using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Restriction;


namespace Framework.Workflow.Domain.Definition
{

    /// <summary>
    /// Переход объекта из одного состояния в другое, посредством получения события
    /// </summary>
    /// <remarks>
    /// Переход описывается следующими параметрами: исходное и конечное состояния и событие, которое инициирует переход
    /// </remarks>
    [WorkflowViewDomainObject]
    [WorkflowEditDomainObject]
    [BLLViewRole, BLLSaveRole(AllowCreate = false), BLLRemoveRole]
    public class Transition : WorkflowItemBase, IDetail<Workflow>, IMaster<TransitionAction>, IWorkflowElement
    {
        private readonly Workflow workflow;

        private StateBase from;

        private StateBase to;

        private readonly ICollection<TransitionAction> postActions = new List<TransitionAction>();

        private Event triggerEvent;


        protected Transition()
        {

        }

        /// <summary>
        /// Конструктор создает переход с ссылкой на воркфлоу
        /// </summary>
        /// <param name="workflow">Воркфлоу</param>
        public Transition(Framework.Workflow.Domain.Definition.Workflow workflow)
        {
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));

            this.workflow = workflow;
            this.workflow.AddDetail(this);
        }

        /// <summary>
        /// Воркфлоу, к которому относится переход
        /// </summary>
        public virtual Workflow Workflow
        {
            get { return this.workflow; }
        }

        /// <summary>
        /// Состояние, из которого ведёт переход
        /// </summary>
        [WorkflowElementValidator]
        [RequiredStartValidator]
        public virtual StateBase From
        {
            get { return this.@from; }
            internal protected set { this.@from = value; }
        }

        /// <summary>
        /// Cостояние, в который ведёт переход
        /// </summary>
        [WorkflowElementValidator]
        [RequiredStartValidator]
        public virtual StateBase To
        {
            get { return this.to; }
            set { this.to = value; }
        }

        /// <summary>
        /// Событие, которое инициирует переход
        /// </summary>
        [WorkflowElementValidator]
        [RequiredStartValidator]
        [UniqueElement("Event")]
        public virtual Event TriggerEvent
        {
            get { return this.triggerEvent; }
            set { this.triggerEvent = value; }
        }

        #region IDetail Members

        Workflow IDetail<Workflow>.Master
        {
            get { return this.Workflow; }
        }

        public virtual IEnumerable<TransitionAction> PostActions
        {
            get { return this.postActions; }
        }

        ICollection<TransitionAction> IMaster<TransitionAction>.Details
        {
            get { return (ICollection<TransitionAction>)this.PostActions; }
        }


        #endregion
    }
}