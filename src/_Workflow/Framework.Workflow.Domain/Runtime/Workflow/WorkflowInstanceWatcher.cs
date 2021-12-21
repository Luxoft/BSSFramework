using System;

using Framework.Persistent;

namespace Framework.Workflow.Domain.Runtime
{

    /// <summary>
    /// Право пользователя на просмотр объектов, минуя систему прав
    /// </summary>
    public class WorkflowInstanceWatcher : Principal, IDetail<WorkflowInstance>
    {
        private readonly WorkflowInstance workflowInstance;


        protected WorkflowInstanceWatcher()
        {

        }

        /// <summary>
        /// Конструтор создает экземпляр сотрудника с ссылкой на экземпляр воркфлоу
        /// </summary>
        /// <param name="workflowInstance">Воркфлоу инстанс</param>
        public WorkflowInstanceWatcher(WorkflowInstance workflowInstance)
        {
            if (workflowInstance == null) throw new ArgumentNullException(nameof(workflowInstance));

            this.workflowInstance = workflowInstance;
            this.workflowInstance.AddDetail(this);
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