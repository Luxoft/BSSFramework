using System;

using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Workflow.Domain.Definition
{

    /// <summary>
    /// Связка между воркфлоу и дочерним воркфлоу
    /// </summary>
    [WorkflowViewDomainObject]
    public class ParallelStateStartItem : AuditPersistentDomainObjectBase, IDetail<ParallelState>, IWorkflowElement
    {
        private readonly ParallelState state;

        private Workflow subWorkflow;

        private WorkflowLambda factory;


        protected ParallelStateStartItem()
        {

        }

        /// <summary>
        /// Конструктор создает связку между воркфлоу и дочерним воркфлоу с ссылкой на параллельное состояние
        /// </summary>
        /// <param name="state">Параллельное состояние</param>
        public ParallelStateStartItem(ParallelState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            this.state = state;
            this.state.AddDetail(this);
        }

        /// <summary>
        /// Параллельное состояние
        /// </summary>
        public virtual ParallelState State
        {
            get { return this.state; }
        }

        /// <summary>
        /// Дочернее воркфлоу
        /// </summary>
        [WorkflowElementValidator]
        [Required]
        [WorkflowTargetSystemValidator]
        public virtual Workflow SubWorkflow
        {
            get { return this.subWorkflow; }
            set { this.subWorkflow = value; }
        }

        /// <summary>
        /// Лямбда типа "Factory"
        /// </summary>
        /// <remarks>
        /// Параллельное состояние опционально имеет лямбду "Factory", когда параметров больше одного и/или количество создаваемых инстансов дочерних воркфлоу динамическое
        /// </remarks>
        [WorkflowElementValidator]
        public virtual WorkflowLambda Factory
        {
            get { return this.factory; }
            set { this.factory = value; }
        }



        #region IDetail<ParallelState> Members

        ParallelState IDetail<ParallelState>.Master
        {
            get { return this.State; }
        }

        #endregion

        #region IWorkflowElement Members

        Workflow IWorkflowElement.Workflow
        {
            get { return this.State.Workflow; }
        }

        #endregion
    }
}