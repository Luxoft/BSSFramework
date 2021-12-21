using System;

using Framework.Persistent;

namespace Framework.Workflow.Domain.Runtime
{

    /// <summary>
    /// Экземпляр параметра воркфлоу
    /// </summary>
    public class WorkflowInstanceParameter : ParameterInstance<Definition.WorkflowParameter>, IDetail<WorkflowInstance>
    {
        private readonly WorkflowInstance workflowInstance;

        /// <summary>
        /// Конструктор создает экземпляр параметра воркфлоу с ссылкой на экземпляр воркфлоу
        /// </summary>
        /// <param name="workflowInstance">Воркфлоу инстанс</param>
        public WorkflowInstanceParameter(WorkflowInstance workflowInstance)
        {
            if (workflowInstance == null) throw new ArgumentNullException(nameof(workflowInstance));

            this.workflowInstance = workflowInstance;
            this.workflowInstance.AddDetail(this);
        }

        protected WorkflowInstanceParameter()
        {
        }

        /// <summary>
        /// Экземпляр воркфлоу
        /// </summary>
        public override WorkflowInstance WorkflowInstance
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