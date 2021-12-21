using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Restriction;
using Framework.Validation;

using JetBrains.Annotations;

namespace Framework.Workflow.Domain.Definition
{

    /// <summary>
    /// Воркфлоу
    /// </summary>
    /// <remarks>
    /// Бизнес-процесс, описывающий жизненный цикл объекта через состояния, условия и переходы
    /// </remarks>
    [WorkflowViewDomainObject]
    [WorkflowEditDomainObject]
    [BLLViewRole, BLLSaveRole(CustomImplementation = true), BLLRemoveRole]
    //[UniqueGroup]
    public partial class Workflow : WorkflowItemBase,

        IMaster<WorkflowParameter>,

        IMaster<State>,
        IMaster<ConditionState>,
        IMaster<ParallelState>,

        IMaster<Transition>,

        IMaster<Role>,

        IMaster<WorkflowSource>,

        IMaster<StartWorkflowDomainObjectCondition>,
        IMaster<WorkflowLambda>,

        IMaster<WorkflowMetadata>,

        IMaster<Workflow>,
        IDetail<Workflow>,

        IDefaultHierarchicalPersistentDomainObjectBase<Workflow>,

        IParametersContainer<WorkflowParameter>,

        IAttachmentContainerHeader,

        ITargetSystemElement<TargetSystem>,

        IWorkflowElement,

        IRoleSource
    {
        private readonly ICollection<WorkflowParameter> parameters = new List<WorkflowParameter>();

        private readonly ICollection<State> states = new List<State>();

        private readonly ICollection<ConditionState> conditionStates = new List<ConditionState>();

        private readonly ICollection<ParallelState> parallelStates = new List<ParallelState>();

        private readonly ICollection<Transition> transitions = new List<Transition>();


        private readonly ICollection<Role> roles = new List<Role>();

        private readonly ICollection<StartWorkflowDomainObjectCondition> startConditions = new List<StartWorkflowDomainObjectCondition>();

        private readonly ICollection<WorkflowLambda> lambdas = new List<WorkflowLambda>();

        private readonly ICollection<Workflow> subWorkflows = new List<Workflow>();

        private readonly ICollection<WorkflowSource> sources = new List<WorkflowSource>();

        private readonly ICollection<WorkflowMetadata> metadatas = new List<WorkflowMetadata>();

        private readonly Workflow owner;

        private DomainType domainType;

        private WorkflowLambda activeCondition;


        private bool hasAttachments;

        private bool isValid;

        private string validationError;

        private bool autoRemoveWithDomainObject;

        /// <summary>
        /// Конструктор создает дочерние воркфлоу с ссылкой на воркфлоу
        /// </summary>
        /// <param name="owner">Воркфлоу</param>
        public Workflow([NotNull] Workflow owner)
        {
            if (owner == null) throw new ArgumentNullException(nameof(owner));

            this.owner = owner;
            this.owner.AddDetail(this);
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Workflow()
        {
        }

        /// <summary>
        /// Родительский воркфлоу
        /// </summary>
        public virtual Workflow Owner
        {
            get { return this.owner; }
        }

        /// <summary>
        /// Коллекция лямбд воркфлоу
        /// </summary>
        [UniqueGroup]
        [MappingPriority(-2)]
        public virtual IEnumerable<WorkflowLambda> Lambdas
        {
            get { return this.lambdas; }
        }

        /// <summary>
        /// Коллекция дочерних ворфлоу
        /// </summary>
        [UniqueGroup]
        [DetailRole(true)]
        [Mapping(CascadeMode = CascadeMode.Enabled)]
        [IgnoreFetch]
        [MappingPriority(-1)]
        public virtual IEnumerable<Workflow> SubWorkflows
        {
            get { return this.subWorkflows; }
        }

        /// <summary>
        /// Коллекция ролей воркфлоу
        /// </summary>
        [UniqueGroup]
        [UniqueGroup("Security")]
        [MappingPriority(-1)]
        public virtual IEnumerable<Role> Roles
        {
            get { return this.roles; }
        }

        #region States

        /// <summary>
        /// Коллекция состояний воркфлоу
        /// </summary>
        [UniqueGroup]
        [IgnoreFetch]
        public virtual IEnumerable<State> States
        {
            get { return this.states; }
        }


        /// <summary>
        /// Коллекция условных состояний воркфлоу
        /// </summary>
        [UniqueGroup]
        [IgnoreFetch]
        public virtual IEnumerable<ConditionState> ConditionStates
        {
            get { return this.conditionStates; }
        }

        /// <summary>
        /// Коллекция параллельных состояний воркфлоу
        /// </summary>
        [UniqueGroup]
        [IgnoreFetch]
        public virtual IEnumerable<ParallelState> ParallelStates
        {
            get { return this.parallelStates; }
        }

        /// <summary>
        /// Коллекция базовых состояний воркфлоу
        /// </summary>
        [CustomSerialization(CustomSerializationMode.Ignore)]
        [UniqueGroup]
        [DetailRole(false)]
        public virtual IEnumerable<StateBase> StateBases
        {
            get { return this.States.Cast<StateBase>().Concat(this.ConditionStates).Concat(this.ParallelStates); }
        }

        #endregion

        /// <summary>
        /// Коллекция параметров воркфлоу
        /// </summary>
        [UniqueGroup]
        public virtual IEnumerable<WorkflowParameter> Parameters
        {
            get { return this.parameters; }
        }

        /// <summary>
        /// Коллекция стартовых условий воркфлоу
        /// </summary>
        [UniqueGroup]
        public virtual IEnumerable<StartWorkflowDomainObjectCondition> StartConditions
        {
            get { return this.startConditions; }
        }

        /// <summary>
        /// Коллекция переходов между состояниями воркфлоу
        /// </summary>
        [UniqueGroup]
        [UniqueCollectionValidator(GroupKey = "Event", OperationContext = WorkflowOperationContextC.Start)]
        public virtual IEnumerable<Transition> Transitions
        {
            get { return this.transitions; }
        }

        /// <summary>
        /// Коллекция поиска экземпляра по доменному объекту воркфлоу
        /// </summary>
        [UniqueGroup]
        public virtual IEnumerable<WorkflowSource> Sources
        {
            get { return this.sources; }
        }

        /// <summary>
        /// Коллекция метеданных воркфлоу
        /// </summary>
        [UniqueGroup]
        public virtual IEnumerable<WorkflowMetadata> Metadatas
        {
            get { return this.metadatas; }
        }

        /// <summary>
        /// Вычисляемая через состояния коллекция задач воркфлоу
        /// </summary>
        [CustomSerialization(CustomSerializationMode.Ignore)]
        public virtual IEnumerable<Task> Tasks
        {
            get { return this.States.SelectMany(s => s.Tasks); }
        }

        /// <summary>
        /// Вычисляемая через доменный тип целевая система
        /// </summary>
        [ExpandPath("DomainType.TargetSystem")]
        public virtual TargetSystem TargetSystem
        {
            get { return this.DomainType.Maybe(dt => dt.TargetSystem); }
        }

        /// <summary>
        /// Доменный тип
        /// </summary>
        [Required]
        public virtual DomainType DomainType
        {
            get { return this.domainType; }
            internal protected set { this.domainType = value; }
        }

        /// <summary>
        /// Лямбда, которая переключает признак активности у экземпляров воркфлоу
        /// </summary>
        /// <remarks>
        /// Она необходимо для того, чтобы всем отработавшим экземплярам воркфлоу поставить признак "Active=False"
        /// Это позволит уменьшить нагрузку на систему и увеличить производительность
        /// </remarks>
        public virtual WorkflowLambda ActiveCondition
        {
            get { return this.activeCondition; }
            set { this.activeCondition = value; }
        }

        /// <summary>
        /// Фильтр с именем "Default"
        /// </summary>
        [CustomSerialization(CustomSerializationMode.Ignore)]
        public virtual WorkflowSource DefaultSource
        {
            get { return this.Sources.GetByName(WorkflowSource.DefaultName, false); }
        }

        /// <summary>
        /// Признак наличия аттачмента у воркфлоу
        /// </summary>
        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        public virtual bool HasAttachments
        {
            get { return this.hasAttachments; }
            set { this.hasAttachments = value; }
        }

        /// <summary>
        /// Признак того, что экземпляр воркфлоу автоматически удаляется вместе с доменным объектом, по которому он запущен
        /// </summary>
        public virtual bool AutoRemoveWithDomainObject
        {
            get { return this.autoRemoveWithDomainObject; }
            set { this.autoRemoveWithDomainObject = value; }
        }

        /// <summary>
        /// Признак активного/неактивного воркфлоу, по которому нужно включить/отключить работу экземпляров воркфлоу
        /// </summary>
        [CustomSerialization(CustomSerializationMode.Normal)]
        public override bool Active
        {
            get { return base.Active; }
            set { base.Active = value; }
        }

        /// <summary>
        /// Признак наличия ошибки в воркфлоу (true - ошибки нет)
        /// </summary>
        /// <remarks>
        /// Валидность вычисляется на сервере и проверяет корректность, без него воркфлоу также будет считаться неактивным
        /// Последнюю ошибку можно увидеть <see cref="ValidationError"/>
        /// </remarks>
        public virtual bool IsValid
        {
            get { return this.isValid; }
            internal protected set { this.isValid = value; }
        }

        /// <summary>
        /// Последняя ошибка в воркфлоу
        /// </summary>
        [MaxLength]
        public virtual string ValidationError
        {
            get { return this.validationError.TrimNull(); }
            protected internal set { this.validationError = value.TrimNull(); }
        }

        /// <summary>
        /// Вычисляемый через ролительский воркфлоу признак того, что воркфлоу является корневым
        /// </summary>
        [CustomSerialization(CustomSerializationMode.Ignore)]
        public virtual bool IsRoot
        {
            get { return this.Owner == null; }
        }


        /// <summary>
        /// Возвращает коллекцию ролей без дубликатов
        /// </summary>
        /// <returns>Коллекция ролей</returns>
        /// <remarks>
        /// Унификация под интерфейсный метод
        /// </remarks>
        public virtual IEnumerable<Role> GetUsingRoles()
        {
            return this.Tasks.SelectMany(task => task.GetUsingRoles()).Distinct();
        }

        /// <summary>
        /// Возвращает параметр воркфлоу типа "Domain Object", связанный с ролью
        /// </summary>
        /// <param name="role">Роль</param>
        /// <returns>Параметр воркфлоу типа "Domain Object"</returns>
        public virtual WorkflowParameter GetParameter(WorkflowParameterRole role)
        {
            return this.Parameters.SingleOrDefault(parameter => parameter.Role == role);
        }

        /// <summary>
        /// Параметр воркфлоу
        /// </summary>
        [CustomSerialization(CustomSerializationMode.Ignore)]
        public virtual WorkflowParameter DomainObjectParameter
        {
            get { return this.GetParameter(WorkflowParameterRole.DomainObject); }
        }


        #region IMaster<State> Members

        ICollection<State> IMaster<State>.Details
        {
            get { return (ICollection<State>)this.States; }
        }

        #endregion

        #region IMaster<ConditionState> Members

        ICollection<ConditionState> IMaster<ConditionState>.Details
        {
            get { return (ICollection<ConditionState>)this.ConditionStates; }
        }

        #endregion

        #region IMaster<ParallelState> Members

        ICollection<ParallelState> IMaster<ParallelState>.Details
        {
            get { return (ICollection<ParallelState>)this.ParallelStates; }
        }

        #endregion

        #region IMaster<WorkflowParameter> Members

        ICollection<WorkflowParameter> IMaster<WorkflowParameter>.Details
        {
            get { return (ICollection<WorkflowParameter>)this.Parameters; }
        }

        #endregion

        #region IMaster<Transition> Members

        ICollection<Transition> IMaster<Transition>.Details
        {
            get { return (ICollection<Transition>)this.Transitions; }
        }

        #endregion

        #region IMaster<Role> Members

        ICollection<Role> IMaster<Role>.Details
        {
            get { return (ICollection<Role>)this.Roles; }
        }

        #endregion

        #region IMaster<WorkflowInstanceSource> Members

        ICollection<WorkflowSource> IMaster<WorkflowSource>.Details
        {
            get { return (ICollection<WorkflowSource>)this.Sources; }
        }

        #endregion

        #region IMaster<StartWorkflowDomainObjectCondition> Members

        ICollection<StartWorkflowDomainObjectCondition> IMaster<StartWorkflowDomainObjectCondition>.Details
        {
            get { return (ICollection<StartWorkflowDomainObjectCondition>)this.StartConditions; }
        }

        #endregion

        #region IMaster<WorkflowLambda> Members

        ICollection<WorkflowLambda> IMaster<WorkflowLambda>.Details
        {
            get { return (ICollection<WorkflowLambda>)this.Lambdas; }
        }

        #endregion

        #region IMaster<Workflow> Members

        ICollection<Workflow> IMaster<Workflow>.Details
        {
            get { return (ICollection<Workflow>)this.SubWorkflows; }
        }

        #endregion

        #region IMaster<TaskMetadata> Members

        ICollection<WorkflowMetadata> IMaster<WorkflowMetadata>.Details
        {
            get { return (ICollection<WorkflowMetadata>)this.Metadatas; }
        }

        #endregion

        #region IDetail<Workflow> Members

        Workflow IDetail<Workflow>.Master
        {
            get { return this.Owner; }
        }

        #endregion

        #region IParentSource<Workflow> IChildrenSource<Workflow> Members

        Workflow IParentSource<Workflow>.Parent
        {
            get { return this.Owner; }
        }

        IEnumerable<Workflow> IChildrenSource<Workflow>.Children
        {
            get { return this.SubWorkflows; }
        }

        #endregion

        #region IWorkflowElement Members

        [ExpandPath("Owner")]
        Workflow IWorkflowElement.Workflow
        {
            get { return this.Owner; }
        }

        #endregion
    }
}
