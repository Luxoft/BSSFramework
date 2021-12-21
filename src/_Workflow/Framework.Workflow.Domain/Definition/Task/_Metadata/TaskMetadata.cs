using System;

using Framework.Persistent;

namespace Framework.Workflow.Domain.Definition
{
    /// <summary>
    /// Метаданные задачи
    /// </summary>
    public partial class TaskMetadata : ObjectMetadata, IWorkflowElement, IDetail<Task>
    {
        private readonly Task task;


        protected TaskMetadata()
        {
        }

        /// <summary>
        /// Конструктор создает метаданные задачи с ссылкой на задачу
        /// </summary>
        /// <param name="task">Задача</param>
        public TaskMetadata(Task task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));

            this.task = task;
            this.task.AddDetail(this);
        }

        /// <summary>
        /// Задача метаданных
        /// </summary>
        public virtual Task Task => this.task;

        #region IWorkflowElement Members

        [ExpandPath("Task.Workflow")]
        Workflow IWorkflowElement.Workflow => this.Task.Workflow;

        #endregion

        #region IDetail<Task> Members

        Task IDetail<Task>.Master => this.Task;

        #endregion
    }
}