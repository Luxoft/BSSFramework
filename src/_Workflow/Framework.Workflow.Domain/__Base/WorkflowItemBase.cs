using Framework.Core;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.Workflow.Domain
{
    /// <summary>
    /// Базовый класс для воркфлоу объектов
    /// </summary>
    public abstract class WorkflowItemBase : AuditPersistentDomainObjectBase, IWorkflowItemBase
    {
        private string name;

        private string description;


        protected WorkflowItemBase()
        {
        }

        /// <summary>
        /// Имя объекта
        /// </summary>
        [VisualIdentity]
        [Required]
        [UniqueElement]
        public virtual string Name
        {
            get { return this.name.TrimNull(); }
            set { this.name = value.TrimNull(); }
        }

        /// <summary>
        /// Описание объекта
        /// </summary>
        public virtual string Description
        {
            get { return this.description.TrimNull(); }
            set { this.description = value.TrimNull(); }
        }


        public override string ToString()
        {
            return this.Name;
        }
    }
}