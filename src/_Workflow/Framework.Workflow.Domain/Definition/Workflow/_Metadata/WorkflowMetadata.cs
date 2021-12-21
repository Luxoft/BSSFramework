using System;

using Framework.Persistent;
using Framework.Projection.Contract;

namespace Framework.Workflow.Domain.Definition
{

    /// <summary>
    /// Метаданные воркфлоу
    /// </summary>
    public partial class WorkflowMetadata : ObjectMetadata, IWorkflowElement, IDetail<Workflow>
    {
        private readonly Workflow workflow;


        protected WorkflowMetadata()
        {

        }

        /// <summary>
        /// Конструктор создает метаданные воркфлоу с ссылкой на воркфлоу
        /// </summary>
        /// <param name="workflow">Воркфлоу</param>
        public WorkflowMetadata(Workflow workflow)
        {
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));

            this.workflow = workflow;
            this.workflow.AddDetail(this);
        }

        /// <summary>
        /// Воркфлоу, который содержит метаданные
        /// </summary>
        public virtual Workflow Workflow
        {
            get { return this.workflow; }
        }

        #region IDetail<Workflow> Members

        Workflow IDetail<Workflow>.Master
        {
            get { return this.Workflow; }
        }

        #endregion
    }
}