using System;

using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Workflow.Domain.Definition
{
    /// <summary>
    /// Лямбда-выражение воркфлоу
    /// </summary>
    [WorkflowViewDomainObject]
    [WorkflowEditDomainObject]
    [BLLViewRole, BLLSaveRole(AllowCreate = false), BLLRemoveRole]
    public class WorkflowLambda : WorkflowItemBase, IDetail<Workflow>, IWorkflowElement, ILambdaObject
    {
        private readonly Workflow workflow;

        private string value;


        protected WorkflowLambda()
        {

        }

        /// <summary>
        /// Конструктор создает лямбду-выражение с ссылкой на воркфлоу
        /// </summary>
        /// <param name="workflow">Воркфлоу</param>
        public WorkflowLambda(Workflow workflow)
        {
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));

            this.workflow = workflow;
            this.workflow.AddDetail(this);
        }

        /// <summary>
        /// Воркфлоу, который содержит лямбду-выражение
        /// </summary>
        public virtual Workflow Workflow => this.workflow;

        /// <summary>
        /// Значение лямбды-выражения
        /// </summary>
        [Required]
        [MaxLength]
        public virtual string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        #region IDetail<Workflow> Members

        Workflow IDetail<Workflow>.Master => this.Workflow;

        #endregion
    }
}