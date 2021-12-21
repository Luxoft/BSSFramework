using System;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;

namespace Framework.Workflow.Domain.Definition
{

    /// <summary>
    /// Фильтр поиска экземпляра воркфлоу по доменному объекту
    /// </summary>
    [WorkflowViewDomainObject]
    [WorkflowEditDomainObject]
    [BLLViewRole]
    public partial class WorkflowSource : WorkflowItemBase, IDetail<Workflow>, IWorkflowElement, ITypeObject<DomainType>
    {
        private readonly Workflow workflow;

        private DomainType type;

        private WorkflowLambda elements;

        private WorkflowLambda path;


        /// <summary>
        /// Конструктор создает фильтр с ссылкой на воркфлоу
        /// </summary>
        /// <param name="workflow">Воркфлоу</param>
        public WorkflowSource(Workflow workflow)
        {
            if (workflow == null) throw new ArgumentNullException(nameof(workflow));

            this.workflow = workflow;
            this.workflow.AddDetail(this);
        }

        protected WorkflowSource()
        {
        }

        /// <summary>
        /// Воркфлоу, к которому относится фильтр
        /// </summary>
        public virtual Workflow Workflow
        {
            get { return this.workflow; }
        }

        /// <summary>
        /// Вычисляемый через воркфлоу доменный тип целевого объект поиска
        /// </summary>
        [ExpandPath("Workflow.DomainType")]
        public virtual DomainType WorkflowType
        {
            get { return this.Workflow.DomainType; }
        }

        /// <summary>
        /// Доменный тип исходного объекта, с которого ведется поиск экземпляров воркфлоу
        /// </summary>
        [RequiredStartValidator]
        [WorkflowTargetSystemValidator]
        public virtual DomainType Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        /// <summary>
        /// Способ поиска элементов воркфлоу
        /// </summary>
        [RequiredStartValidator]
        [WorkflowElementValidator]
        public virtual WorkflowLambda Elements
        {
            get { return this.elements; }
            set { this.elements = value; }
        }

        /// <summary>
        /// Визуальная группировка элементов воркфлоу
        /// </summary>
        [WorkflowElementValidator]
        public virtual WorkflowLambda Path
        {
            get { return this.path; }
            set { this.path = value; }
        }

        /// <summary>
        /// Признак указывает, что путь для поиска экземпляров воркфлоу сформирован автоматически
        /// </summary>
        /// <remarks>
        /// В этом пути экземпляр воркфлоу указывает на самого себя
        /// </remarks>
        [CustomSerialization(CustomSerializationMode.Ignore)]
        public virtual bool IsDefault
        {
            get { return this.Name == WorkflowSource.DefaultName; }
        }


        Workflow IDetail<Workflow>.Master
        {
            get { return this.Workflow; }
        }



        public const string DefaultName = "Default";
    }
}