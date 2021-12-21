using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Workflow.Domain.Definition
{

    /// <summary>
    /// Параметр
    /// </summary>
    public abstract class Parameter :
        WorkflowItemBase,
        IWorkflowElement,
        ITypeObject<DomainType>
    {
        private DomainType type;

        private bool allowNull;


        protected Parameter()
        {

        }

        /// <summary>
        /// Тип параметра
        /// </summary>
        [Required]
        [WorkflowTargetSystemValidator]
        public virtual DomainType Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        /// <summary>
        /// Признак того, что воркфлоу автоматически создается вместе с новым доменным типом, которого еще нет в базе (Previous=null)
        /// </summary>
        public virtual bool AllowNull
        {
            get { return this.allowNull; }
            set { this.allowNull = value; }
        }

        #region IWorkflowElement Members

        public abstract Workflow Workflow { get; }

        #endregion
    }
}