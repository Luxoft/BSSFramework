using System;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;

namespace Framework.Workflow.Domain.Definition
{
    /// <summary>
    /// Событие, после которого происходит смена состояния у бизнес-объекта
    /// </summary>
    /// <remarks>
    /// Движение объекта воркфлоу совершаются только через следующие события:
    /// 1) <see cref="CommandEvent"/>
    /// 2) <see cref="ConditionStateEvent"/>
    /// 3) <see cref="ParallelStateFinalEvent"/>
    /// 4) <see cref="StateTimeoutEvent"/>
    /// 5) <see cref="StateDomainObjectEvent"/>
    /// Переход, подписанный на событие, переводит бизнес-объект в новое состояние
    /// </remarks>
    [WorkflowViewDomainObject]
    [BLLViewRole]
    public class Event : WorkflowItemBase, IWorkflowElement
    {
        protected Event()
        {

        }

        /// <summary>
        /// Состояние события
        /// </summary>
        [CustomSerialization(CustomSerializationMode.Ignore)]
        public virtual StateBase SourceState
        {
            get { return null; }
        }

        public virtual void RecalcName()
        {

        }


        #region IWorkflowElement Members

        Workflow IWorkflowElement.Workflow
        {
            get { return null; }
        }

        #endregion
    }

    /// <summary>
    /// Событие, которое относится к элементу воркфлоу
    /// </summary>
    /// <typeparam name="TMaster">Тип мастера</typeparam>
    public abstract class Event<TMaster> : Event, IWorkflowElement, IDetail<TMaster>
        where TMaster : WorkflowItemBase, IWorkflowElement
    {
        private readonly TMaster owner;


        protected Event()
        {

        }

        protected Event(TMaster owner)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));

            this.owner = owner;
        }

        /// <summary>
        /// Элемент воркфлоу, к которому относится событие
        /// </summary>
        public virtual TMaster Owner
        {
            get { return this.owner; }
        }

        /// <summary>
        /// Вычисляемый воркфлоу, для которого создан элемент ворклоу, к которому относится текущее событие
        /// </summary>
        [ExpandPath("Owner.Workflow")]
        public virtual Workflow Workflow
        {
            get { return this.Owner.Workflow; }
        }

        public override void RecalcName()
        {
            this.Name = this.GetNewName();
        }

       /// <summary>
       /// Получение имени
       /// </summary>
       /// <returns>Новое имя</returns>
       protected virtual string GetNewName()
        {
            return this.Owner.Name + "_Event";
        }

        #region IDetail<TMaster> Members

        TMaster IDetail<TMaster>.Master
        {
            get { return this.Owner; }
        }

        #endregion
    }
}