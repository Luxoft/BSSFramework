using System;

using Framework.DomainDriven.BLL;
using Framework.Persistent;

namespace Framework.Workflow.Domain.Runtime
{

    /// <summary>
    /// Экземпляр выполненного перехода между состояниями
    /// </summary>
    [BLLViewRole]
    [WorkflowViewDomainObject]
    public class TransitionInstance : AuditPersistentDomainObjectBase,

        IDetail<WorkflowInstance>,
        IDefinitionDomainObject<Definition.Transition>
    {
        private readonly WorkflowInstance workflowInstance;

        private readonly StateInstance from;
        private readonly StateInstance to;

        private readonly Definition.Transition definition;

        /// <summary>
        /// Конструктор создает экземпляр перехода между состояниями
        /// </summary>
        /// <param name="workflowInstance">Экземпляр воркфлоу</param>
        /// <param name="definition">Definition перехода</param>
        /// <param name="from">Экземпляр состояния, из которого ведёт переход</param>
        /// <param name="to">Экземпляр состояния, в который ведёт переход</param>
        public TransitionInstance(WorkflowInstance workflowInstance, Definition.Transition definition, StateInstance from, StateInstance to)
        {
            if (workflowInstance == null) throw new ArgumentNullException(nameof(workflowInstance));
            if (definition == null) throw new ArgumentNullException(nameof(definition));
            if (from == null) throw new ArgumentNullException(nameof(@from));
            if (to == null) throw new ArgumentNullException(nameof(to));

            this.workflowInstance = workflowInstance;
            this.definition = definition;
            this.from = from;
            this.to = to;

            this.workflowInstance.AddDetail(this);
        }

        protected TransitionInstance()
        {
        }

        /// <summary>
        /// Definition перехода
        /// </summary>
        public virtual Definition.Transition Definition
        {
            get { return this.definition; }
        }

        /// <summary>
        /// Экземпляр состояния, из которого ведёт переход
        /// </summary>
        public virtual StateInstance From
        {
            get { return this.@from; }
        }

        /// <summary>
        /// Экземпляр состояния, в который ведёт переход
        /// </summary>
        public virtual StateInstance To
        {
            get { return this.to; }
        }

        /// <summary>
        /// Экземпляр воркфлоу
        /// </summary>
        public virtual WorkflowInstance WorkflowInstance
        {
            get { return this.workflowInstance; }
        }

        #region IDetail<WorkflowInstance> Members

        WorkflowInstance IDetail<WorkflowInstance>.Master
        {
            get { return this.WorkflowInstance; }
        }

        #endregion
    }
}
